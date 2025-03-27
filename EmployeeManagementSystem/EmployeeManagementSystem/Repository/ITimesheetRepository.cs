using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repository
{
    public interface ITimesheetRepository
    {
        Task<IEnumerable<Timesheet>> GetAllTimesheets();
        Task<IEnumerable<Timesheet>> GetTimesheetsByEmployeeId(int employeeId);
        Task<Timesheet> GetTimesheetById(int timesheetId);
        Task AddTimesheet(Timesheet timesheet);
        Task UpdateTimesheet(Timesheet timesheet);
        Task DeleteTimesheet(int timesheetId);
        
        //method for aggregating employee work hours
        Task<IEnumerable<object>> GetEmployeeWorkHours(string period);
    }
}
