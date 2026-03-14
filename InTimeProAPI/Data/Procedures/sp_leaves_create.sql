CREATE OR ALTER PROCEDURE dbo.sp_leaves_create
    @EmployeeId UNIQUEIDENTIFIER,
    @LeaveType NVARCHAR(30),
    @StartDate DATE,
    @EndDate DATE,
    @Reason NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @EmployeeId AS EmployeeId, @LeaveType AS LeaveType, @StartDate AS StartDate, @EndDate AS EndDate;
END;
