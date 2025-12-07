using System.ComponentModel.DataAnnotations;

namespace Capstone_Project_PROG36944.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        // Navigation: all tasks that belong to this project
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
