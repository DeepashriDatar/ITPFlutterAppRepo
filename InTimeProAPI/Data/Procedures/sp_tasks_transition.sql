CREATE OR ALTER PROCEDURE dbo.sp_tasks_transition
    @TaskId UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER,
    @Action NVARCHAR(20),
    @OccurredAtUtc DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @TaskId AS TaskId, @EmployeeId AS EmployeeId, @Action AS [Action], @OccurredAtUtc AS OccurredAtUtc;
END;
