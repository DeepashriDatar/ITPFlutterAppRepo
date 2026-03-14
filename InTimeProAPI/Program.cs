using InTimeProAPI.Data;
using InTimeProAPI.Services;
using InTimeProAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

const string ApiVersion = "v1";

ApplyConfiguredSecrets(builder.Configuration, builder.Environment);

var rootLogger = LoggerFactory.Create(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
}).CreateLogger("Startup");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection is not configured");

ValidateConnectionStringSecurity(connectionString);

// ── Database ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure(3);
        sql.CommandTimeout(30);
    }));

// ── JWT Authentication ───────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options => AuthorizationPolicyService.Configure(options));

// ── CORS (allow Flutter app) ─────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FlutterApp", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IAlertingService, AlertingService>();
builder.Services.AddScoped<IDataRetentionService, DataRetentionService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TimesheetService>();
builder.Services.AddScoped<LeaveService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>(
    "sql",
    failureStatus: HealthStatus.Unhealthy,
    tags: new[] { "db", "ready" });

if (builder.Environment.IsDevelopment())
{
    builder.Configuration["Secrets:Provider"] = "user-secrets";
}

// ── Controllers & Swagger ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InTimePro API",
        Version = "v1",
        Description = "InTimePro Mobile App Backend API"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Enter: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InTimePro API v1"));
}

app.UseHttpsRedirection();
app.UseCors("FlutterApp");
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// ── Auto-migrate on startup ───────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var retention = scope.ServiceProvider.GetRequiredService<IDataRetentionService>();
    await retention.ApplyRetentionAsync(CancellationToken.None);
}

rootLogger.LogInformation("InTimePro API booted with route base /api/{ApiVersion}", ApiVersion);
app.Run();

static void ApplyConfiguredSecrets(IConfiguration configuration, IHostEnvironment environment)
{
    var provider = configuration["Secrets:Provider"];
    var refs = new[]
    {
        (ConfigPath: "JwtSettings:SecretKey", RefPath: "Secrets:JwtSecretRef"),
        (ConfigPath: "GoogleAuth:ClientSecret", RefPath: "Secrets:GoogleClientSecretRef"),
        (ConfigPath: "MicrosoftAuth:ClientSecret", RefPath: "Secrets:MicrosoftClientSecretRef")
    };

    foreach (var (configPath, refPath) in refs)
    {
        var secretRef = configuration[refPath];
        if (string.IsNullOrWhiteSpace(secretRef))
        {
            continue;
        }

        var secretValue = Environment.GetEnvironmentVariable(secretRef);
        if (!string.IsNullOrWhiteSpace(secretValue))
        {
            configuration[configPath] = secretValue;
            continue;
        }

        if (!environment.IsDevelopment() && string.Equals(provider, "environment", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Required secret '{secretRef}' for '{configPath}' is not configured in environment variables");
        }
    }
}

static void ValidateConnectionStringSecurity(string connectionString)
{
    if (!connectionString.Contains("Encrypt=True", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Connection string must enforce Encrypt=True");
    }

    if (!connectionString.Contains("TrustServerCertificate=False", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Connection string must enforce TrustServerCertificate=False");
    }

    if (!connectionString.Contains("Max Pool Size", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Connection string must set Max Pool Size");
    }
}
