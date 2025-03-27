using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly ApplicationDbContext _context;

        public TimesheetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Timesheet>> GetAllTimesheets()
        {
            return await _context.Timesheets.Include(t => t.Employee).ToListAsync();
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheetsByEmployeeId(int employeeId)
        {
            return await _context.Timesheets.Where(t => t.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<Timesheet> GetTimesheetById(int timesheetId)
        {
            return await _context.Timesheets.FindAsync(timesheetId);
        }

        public async Task AddTimesheet(Timesheet timesheet)
        {
            await _context.Timesheets.AddAsync(timesheet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Update(timesheet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTimesheet(int timesheetId)
        {
            var timesheet = await _context.Timesheets.FindAsync(timesheetId);
            if (timesheet != null)
            {
                _context.Timesheets.Remove(timesheet);
                await _context.SaveChangesAsync();
            }
        }
        //method for calculating total work hours per employee
        public async Task<IEnumerable<object>> GetEmployeeWorkHours(string period)
        {
            DateTime startDate, endDate;

            if (period.ToLower() == "weekly")
            {
                startDate = DateTime.UtcNow.AddDays(-7);
                endDate = DateTime.UtcNow;
            }
            else if (period.ToLower() == "monthly")
            {
                startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                endDate = DateTime.UtcNow;
            }
            else
            {
                throw new ArgumentException("Invalid period. Use 'weekly' or 'monthly'.");
            }

            return await _context.Timesheets
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .GroupBy(t => t.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    TotalHoursWorked = g.Sum(t => t.TotalHours)
                })
                .ToListAsync();
        }
    }
}
