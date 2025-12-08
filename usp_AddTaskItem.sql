USE DotNetClass;
GO

IF OBJECT_ID('dbo.usp_AddTaskItem', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_AddTaskItem;
GO

CREATE PROCEDURE dbo.usp_AddTaskItem
    @Title NVARCHAR(100),
    @Description NVARCHAR(500) = NULL,
    @DueDate DATE = NULL,
    @ProjectId INT,
    @AssignedEmployeeId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.TaskItems
        (Title, Description, DueDate, ProjectId, AssignedEmployeeId)
    VALUES
        (@Title, @Description, @DueDate, @ProjectId, @AssignedEmployeeId);

    SELECT SCOPE_IDENTITY() AS NewTaskItemId;
END;
GO
