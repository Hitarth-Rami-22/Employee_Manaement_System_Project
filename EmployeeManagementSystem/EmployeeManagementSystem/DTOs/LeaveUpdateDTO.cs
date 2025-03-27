namespace EmployeeManagementSystem.DTOs
{
    public class LeaveUpdateDTO
    //Used for admins to approve/reject leave requests.
    {
        public string Status { get; set; } // "Approved" or "Rejected"
    }
}
