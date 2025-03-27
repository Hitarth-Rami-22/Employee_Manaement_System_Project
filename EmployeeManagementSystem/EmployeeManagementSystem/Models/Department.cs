using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
