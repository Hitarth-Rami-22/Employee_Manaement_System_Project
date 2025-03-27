namespace EmployeeManagementSystem.DTOs
{
    public class EmployeeDTO
    //Transfers employee data without the password hash.
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public int DepartmentId { get; set; }
        public string TechStack { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
