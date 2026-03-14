using Microsoft.AspNetCore.Authorization;

namespace InTimeProAPI.Services;

public static class AuthorizationPolicyService
{
    public const string AdminPolicy = "RequireAdmin";
    public const string EmployeePolicy = "RequireEmployee";
    public const string AuditorPolicy = "RequireAuditor";

    public static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(AdminPolicy, policy => policy.RequireRole("Admin"));
        options.AddPolicy(EmployeePolicy, policy => policy.RequireRole("Employee", "Admin"));
        options.AddPolicy(AuditorPolicy, policy => policy.RequireRole("Auditor", "Admin"));
    }
}
