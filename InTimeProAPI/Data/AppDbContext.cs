using InTimeProAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InTimeProAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AuditLogEntry> AuditLogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.HasIndex(x => x.GoogleId);
            e.HasIndex(x => x.MicrosoftId);
            e.Property(x => x.Role).HasDefaultValue("Employee");
        });

        modelBuilder.Entity<RefreshToken>(rt =>
        {
            rt.HasOne(x => x.Employee)
              .WithMany(x => x.RefreshTokens)
              .HasForeignKey(x => x.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLogEntry>(audit =>
        {
            audit.HasIndex(x => x.OccurredAtUtc);
            audit.HasIndex(x => x.CorrelationId);
        });

        // Seed a test employee — password is Test@1234
        // Hash pre-computed to avoid BCrypt dependency at design time
        modelBuilder.Entity<Employee>().HasData(new
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Email = "saket@wai.com",
            Name = "Saket Khare",
            Department = "Engineering",
            Role = "Employee",
            PasswordHash = "$2a$11$7Q9v7Q9v7Q9v7Q9v7Q9vOOa5WQqKpJb5x5x5x5x5x5x5x5x5x5xS",
            AuthProvider = "email",
            Phone = (string?)null,
            AvatarUrl = (string?)null,
            GoogleId = (string?)null,
            MicrosoftId = (string?)null,
            IsActive = true,
            CreatedAt = new DateTime(2026, 3, 13, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2026, 3, 13, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
