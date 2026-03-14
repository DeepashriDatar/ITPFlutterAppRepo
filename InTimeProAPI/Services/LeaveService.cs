using System.Data;
using Microsoft.Data.SqlClient;

namespace InTimeProAPI.Services;

public class LeaveService
{
    private readonly IConfiguration _configuration;

    public LeaveService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task CreateLeaveAsync(Guid employeeId, string leaveType, DateOnly startDate, DateOnly endDate, string? reason, CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand("dbo.sp_leaves_create", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@EmployeeId", employeeId);
        command.Parameters.AddWithValue("@LeaveType", leaveType);
        command.Parameters.AddWithValue("@StartDate", startDate.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@EndDate", endDate.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@Reason", reason ?? string.Empty);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
