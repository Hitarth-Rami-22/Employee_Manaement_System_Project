using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    public interface ITimesheetService
    {
        Task<IEnumerable<Timesheet>> GetAllTimesheets();
        Task<IEnumerable<Timesheet>> GetTimesheetsByEmployeeId(int employeeId);
        Task<Timesheet> GetTimesheetById(int timesheetId);
        Task AddTimesheet(Timesheet timesheet);
        Task UpdateTimesheet(Timesheet timesheet);
        Task DeleteTimesheet(int timesheetId);

        Task ApproveTimesheet(int timesheetId, int adminId);
        Task RejectTimesheet(int timesheetId, int adminId);

        //Method to get work hours for reports
        Task<IEnumerable<object>> GetEmployeeWorkHours(string period);

        //method to export timesheets to Excel
        Task<byte[]> ExportTimesheetsToExcel();
    }
}
