using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Timesheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimesheetId { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalHours { get; set; }

        public string Description { get; set; }
        public string Status { get; set; } = "Pending"; // Default status

        public int? ApprovedBy { get; set; } // Admin who approved/rejected

        public DateTime? ApprovedAt { get; set; } // Approval timestamp

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
