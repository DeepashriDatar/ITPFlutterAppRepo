namespace InTimeProAPI.DTOs;

public record DashboardSummaryDto
{
    public string ClockInTime { get; init; } = "--:--";
    public string ActiveWorkingHours { get; init; } = "0h 00m";
    public string TotalWorkTime { get; init; } = "0h 00m";
    public string ActiveTaskName { get; init; } = "No active task";
    public string ActiveTaskState { get; init; } = "None";
    public int ActiveTaskElapsedSeconds { get; init; }
}
