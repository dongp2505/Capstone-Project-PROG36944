USE DotNetClass;
GO

IF OBJECT_ID('dbo.usp_GetTasksWithDetails', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetTasksWithDetails;
GO

CREATE PROCEDURE dbo.usp_GetTasksWithDetails
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        t.TaskItemId,
        t.Title,
        t.DueDate,
        p.Name AS ProjectName,
        e.FirstName + ' ' + e.LastName AS EmployeeName
    FROM dbo.TaskItems AS t
    INNER JOIN dbo.Projects AS p
        ON t.ProjectId = p.ProjectId
    LEFT JOIN dbo.Employees AS e
        ON t.AssignedEmployeeId = e.EmployeeId
    ORDER BY
        t.DueDate,
        t.Title;
END;
GO
