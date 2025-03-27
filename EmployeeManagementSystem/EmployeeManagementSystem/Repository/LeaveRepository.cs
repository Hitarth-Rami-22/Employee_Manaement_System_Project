using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Leave>> GetAllLeaves()
        {
            return await _context.Leaves.Include(l => l.Employee).ToListAsync();
        }

        public async Task<IEnumerable<Leave>> GetLeavesByEmployeeId(int employeeId)
        {
            return await _context.Leaves.Where(l => l.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<Leave> GetLeaveById(int leaveId)
        {
            return await _context.Leaves.FindAsync(leaveId);
        }

        public async Task AddLeave(Leave leave)
        {
            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeave(Leave leave)
        {
            _context.Leaves.Update(leave);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeave(int leaveId)
        {
            var leave = await _context.Leaves.FindAsync(leaveId);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
            }
        }
    }
}
