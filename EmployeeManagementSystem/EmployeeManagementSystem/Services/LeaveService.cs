using EmployeeManagementSystem.Helpers;
using System.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repository;

namespace EmployeeManagementSystem.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<IEnumerable<Leave>> GetAllLeaves()
        {
            return await _leaveRepository.GetAllLeaves();
        }

        public async Task<IEnumerable<Leave>> GetLeavesByEmployeeId(int employeeId)
        {
            return await _leaveRepository.GetLeavesByEmployeeId(employeeId);
        }

        public async Task<Leave> GetLeaveById(int leaveId)
        {
            return await _leaveRepository.GetLeaveById(leaveId);
        }

        public async Task AddLeave(Leave leave)
        {
            await _leaveRepository.AddLeave(leave);
        }

        public async Task UpdateLeave(Leave leave)
        {
            await _leaveRepository.UpdateLeave(leave);
        }

        public async Task DeleteLeave(int leaveId)
        {
            await _leaveRepository.DeleteLeave(leaveId);
        }

        // Approve leave request
        public async Task ApproveLeave(int leaveId, int adminId)
        {
            var leave = await _leaveRepository.GetLeaveById(leaveId);
            if (leave == null || leave.Status != "Pending")
                throw new Exception("Leave request not found or already processed.");

            leave.Status = "Approved";
            leave.ApprovedBy = adminId;
            leave.ApprovedAt = DateTime.UtcNow;

            await _leaveRepository.UpdateLeave(leave);
        }

        // Reject leave request
        public async Task RejectLeave(int leaveId, int adminId)
        {
            var leave = await _leaveRepository.GetLeaveById(leaveId);
            if (leave == null || leave.Status != "Pending")
                throw new Exception("Leave request not found or already processed.");

            leave.Status = "Rejected";
            leave.ApprovedBy = adminId;
            leave.ApprovedAt = DateTime.UtcNow;

            await _leaveRepository.UpdateLeave(leave);
        }
        //Method: Export Leaves to Excel
        public async Task<byte[]> ExportLeavesToExcel()
        {
            var leaves = await _leaveRepository.GetAllLeaves();

            DataTable dt = new DataTable();
            dt.Columns.Add("LeaveId");
            dt.Columns.Add("EmployeeId");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("EndDate");
            dt.Columns.Add("LeaveType");
            dt.Columns.Add("Status");

            foreach (var l in leaves)
            {
                dt.Rows.Add(l.LeaveId, l.EmployeeId, l.StartDate, l.EndDate, l.LeaveType, l.Status);
            }

            return ExcelHelper.GenerateExcelFile(dt, "Leaves");

        }
    }
}
