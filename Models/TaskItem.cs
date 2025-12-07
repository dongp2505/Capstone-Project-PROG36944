using System.ComponentModel.DataAnnotations;

namespace Capstone_Project_PROG36944.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskItemId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Task Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime? DueDate { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        public Project? Project { get; set; }

        [Display(Name = "Assigned Employee")]
        public int? AssignedEmployeeId { get; set; }

        public Employee? AssignedEmployee { get; set; }
    }
}
