using System.Data;
using Microsoft.Data.SqlClient;

namespace InTimeProAPI.Services;

public class TaskService
{
    private readonly IConfiguration _configuration;

    public TaskService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task TransitionTaskAsync(Guid taskId, Guid employeeId, string action, DateTime occurredAtUtc, CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand("dbo.sp_tasks_transition", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@TaskId", taskId);
        command.Parameters.AddWithValue("@EmployeeId", employeeId);
        command.Parameters.AddWithValue("@Action", action);
        command.Parameters.AddWithValue("@OccurredAtUtc", occurredAtUtc);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
