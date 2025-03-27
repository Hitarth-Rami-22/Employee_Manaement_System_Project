using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        // GET: api/timesheet - Employees can view their own timesheets
        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<IEnumerable<Timesheet>>> GetTimesheets()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userRole == "Employee")
            {
                var employeeTimesheets = await _timesheetService.GetTimesheetsByEmployeeId(GetUserId());
                return Ok(employeeTimesheets);
            }

            var allTimesheets = await _timesheetService.GetAllTimesheets();
            return Ok(allTimesheets);
        }

        // GET: api/timesheet/{id} - Employees and Admins can fetch timesheet by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<Timesheet>> GetTimesheetById(int id)
        {
            var timesheet = await _timesheetService.GetTimesheetById(id);
            if (timesheet == null)
                return NotFound(new { message = "Timesheet not found" });

            return Ok(timesheet);
        }

        // POST: api/timesheet - Employees can log working hours
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Timesheet>> AddTimesheet([FromBody] Timesheet timesheet)
        {
            timesheet.EmployeeId = GetUserId();  // Ensure employee is adding their own timesheet
            await _timesheetService.AddTimesheet(timesheet);
            return CreatedAtAction(nameof(GetTimesheetById), new { id = timesheet.TimesheetId }, timesheet);
        }

        // PUT: api/timesheet/{id} - Employees can update their own timesheets
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateTimesheet(int id, [FromBody] Timesheet timesheet)
        {
            if (id != timesheet.TimesheetId)
                return BadRequest(new { message = "Timesheet ID mismatch" });

            var existingTimesheet = await _timesheetService.GetTimesheetById(id);
            if (existingTimesheet == null)
                return NotFound(new { message = "Timesheet not found" });

            if (existingTimesheet.EmployeeId != GetUserId())
                return Unauthorized(new { message = "You can only update your own timesheets" });

            await _timesheetService.UpdateTimesheet(timesheet);
            return NoContent();
        }

        // DELETE: api/timesheet/{id} - Employees can delete their own timesheets
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteTimesheet(int id)
        {
            var existingTimesheet = await _timesheetService.GetTimesheetById(id);
            if (existingTimesheet == null)
                return NotFound(new { message = "Timesheet not found" });

            if (existingTimesheet.EmployeeId != GetUserId())
                return Unauthorized(new { message = "You can only delete your own timesheets" });

            await _timesheetService.DeleteTimesheet(id);
            return NoContent();
        }

        // PUT: api/timesheet/approve/{id} - Admin approves a timesheet
        [Authorize(Roles = "Admin")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveTimesheet(int id)
        {
            var timesheet = await _timesheetService.GetTimesheetById(id);
            if (timesheet == null)
                return NotFound(new { message = "Timesheet not found" });

            timesheet.Status = "Approved";
            await _timesheetService.UpdateTimesheet(timesheet);
            return NoContent();
        }

        // PUT: api/timesheet/reject/{id} - Admin rejects a timesheet
        [Authorize(Roles = "Admin")]
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectTimesheet(int id)
        {
            var timesheet = await _timesheetService.GetTimesheetById(id);
            if (timesheet == null)
                return NotFound(new { message = "Timesheet not found" });

            timesheet.Status = "Rejected";
            await _timesheetService.UpdateTimesheet(timesheet);
            return NoContent();
        }

        // Helper Method to Get Logged-in Employee ID
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

    }
}
