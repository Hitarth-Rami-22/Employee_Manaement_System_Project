﻿namespace EmployeeManagementSystem.DTOs
{
    public class LeaveCreateDTO
    //Used for leave application.
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
    }
}
