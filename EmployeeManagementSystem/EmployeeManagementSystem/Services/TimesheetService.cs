using EmployeeManagementSystem.Helpers;//helper class for Excel generation
using System.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repository;

namespace EmployeeManagementSystem.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public async Task<IEnumerable<Timesheet>> GetAllTimesheets()
        {
            return await _timesheetRepository.GetAllTimesheets();
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheetsByEmployeeId(int employeeId)
        {
            return await _timesheetRepository.GetTimesheetsByEmployeeId(employeeId);
        }

        public async Task<Timesheet> GetTimesheetById(int timesheetId)
        {
            return await _timesheetRepository.GetTimesheetById(timesheetId);
        }

        public async Task AddTimesheet(Timesheet timesheet)
        {
            await _timesheetRepository.AddTimesheet(timesheet);
        }

        public async Task UpdateTimesheet(Timesheet timesheet)
        {
            await _timesheetRepository.UpdateTimesheet(timesheet);
        }

        public async Task DeleteTimesheet(int timesheetId)
        {
            await _timesheetRepository.DeleteTimesheet(timesheetId);
        }

        public async Task<IEnumerable<object>> GetEmployeeWorkHours(string period)
        {
            return await _timesheetRepository.GetEmployeeWorkHours(period);
        }
        public async Task ApproveTimesheet(int timesheetId, int adminId)
        {
            var timesheet = await _timesheetRepository.GetTimesheetById(timesheetId);
            if (timesheet == null || timesheet.Status != "Pending")
                throw new Exception("Timesheet not found or already processed.");

            timesheet.Status = "Approved";
            timesheet.ApprovedBy = adminId;
            timesheet.ApprovedAt = DateTime.UtcNow;

            await _timesheetRepository.UpdateTimesheet(timesheet);
        }

        public async Task RejectTimesheet(int timesheetId, int adminId)
        {
            var timesheet = await _timesheetRepository.GetTimesheetById(timesheetId);
            if (timesheet == null || timesheet.Status != "Pending")
                throw new Exception("Timesheet not found or already processed.");

            timesheet.Status = "Rejected";
            timesheet.ApprovedBy = adminId;
            timesheet.ApprovedAt = DateTime.UtcNow;

            await _timesheetRepository.UpdateTimesheet(timesheet);
        }

        //Method: Export Timesheets to Excel
        public async Task<byte[]> ExportTimesheetsToExcel()
        {
            var timesheets = await _timesheetRepository.GetAllTimesheets();

            DataTable dt = new DataTable();
            dt.Columns.Add("TimesheetId");
            dt.Columns.Add("EmployeeId");
            dt.Columns.Add("Date");
            dt.Columns.Add("StartTime");
            dt.Columns.Add("EndTime");
            dt.Columns.Add("TotalHours");
            dt.Columns.Add("Description");

            foreach (var t in timesheets)
            {
                //dt.Rows.Add(t.TimesheetId, t.EmployeeId, t.Date, t.StartTime, t.EndTime, t.TotalHours, t.Description);
                dt.Rows.Add(t.TimesheetId, t.EmployeeId, t.Date.ToString("yyyy-MM-dd"),
                           t.StartTime.ToString(@"hh\:mm"), t.EndTime.ToString(@"hh\:mm"),
                           t.TotalHours, t.Description);
            }

            return ExcelHelper.GenerateExcelFile(dt, "Timesheets");
        }
    }
}
