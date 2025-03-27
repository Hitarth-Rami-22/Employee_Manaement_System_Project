using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    public interface ILeaveService
    {
        Task<IEnumerable<Leave>> GetAllLeaves();
        Task<IEnumerable<Leave>> GetLeavesByEmployeeId(int employeeId);
        Task<Leave> GetLeaveById(int leaveId);
        Task AddLeave(Leave leave);
        Task UpdateLeave(Leave leave);
        Task DeleteLeave(int leaveId);
        Task ApproveLeave(int leaveId, int adminId);
        Task RejectLeave(int leaveId, int adminId);


        Task<byte[]> ExportLeavesToExcel();
    }
}
