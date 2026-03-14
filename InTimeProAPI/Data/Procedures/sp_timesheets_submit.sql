CREATE OR ALTER PROCEDURE dbo.sp_timesheets_submit
    @EmployeeId UNIQUEIDENTIFIER,
    @Date DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @EmployeeId AS EmployeeId, @Date AS [Date], 'Submitted' AS [Status];
END;
