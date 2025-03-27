namespace EmployeeManagementSystem.DTOs
{
    public class TimesheetCreateDTO
    //Used for employees to log work hours
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public string Description { get; set; }
    }
}
