namespace EmployeeManagementSystem.DTOs
{
    public class TimesheetUpdateDTO
    //Used for employees to edit timesheet entries.
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public string Description { get; set; }
    }
}
