using System.Data;
using Microsoft.Data.SqlClient;

namespace InTimeProAPI.Services;

public class TimesheetService
{
    private readonly IConfiguration _configuration;

    public TimesheetService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SubmitDailyAsync(Guid employeeId, DateOnly date, CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand("dbo.sp_timesheets_submit", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@EmployeeId", employeeId);
        command.Parameters.AddWithValue("@Date", date.ToDateTime(TimeOnly.MinValue));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
