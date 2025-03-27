using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Leave
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveId { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, MaxLength(50)]
        public string LeaveType { get; set; }

        public string Reason { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Default status is "Pending"
        public int? ApprovedBy { get; set; } // Admin who approved/rejected

        public DateTime? ApprovedAt { get; set; } // Approval timestamp
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}
