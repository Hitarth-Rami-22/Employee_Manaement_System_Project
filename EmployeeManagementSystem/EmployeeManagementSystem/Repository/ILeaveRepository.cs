using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repository
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<Leave>> GetAllLeaves();
        Task<IEnumerable<Leave>> GetLeavesByEmployeeId(int employeeId);
        Task<Leave> GetLeaveById(int leaveId);
        Task AddLeave(Leave leave);
        Task UpdateLeave(Leave leave);
        Task DeleteLeave(int leaveId);

    }
}
