namespace EmployeeManagementSystem.DTOs
{
    public class DepartmentDTO
    //Simplifies department data transfer.
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
