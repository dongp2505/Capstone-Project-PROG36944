using System;

namespace Capstone_Project_PROG36944.Models
{
    public class TaskItemSummary
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? EmployeeName { get; set; }
    }
}
