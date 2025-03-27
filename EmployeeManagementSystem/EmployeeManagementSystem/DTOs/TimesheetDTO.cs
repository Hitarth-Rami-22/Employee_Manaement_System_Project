﻿namespace EmployeeManagementSystem.DTOs
{
    public class TimesheetDTO
    //Transfers timesheet details.
    {
        public int TimesheetId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalHours { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
