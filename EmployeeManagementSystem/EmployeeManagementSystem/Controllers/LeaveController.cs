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
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        // GET: api/leave - Employees can view their own leave requests
        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaves()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Employee")
            {
                var employeeLeaves = await _leaveService.GetLeavesByEmployeeId(GetUserId());
                return Ok(employeeLeaves);
            }

            var allLeaves = await _leaveService.GetAllLeaves();
            return Ok(allLeaves);
        }

        // GET: api/leave/{id} - Fetch leave request by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult<Leave>> GetLeaveById(int id)
        {
            var leave = await _leaveService.GetLeaveById(id);
            if (leave == null)
                return NotFound(new { message = "Leave request not found" });

            return Ok(leave);
        }

        // POST: api/leave - Employees apply for leave
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Leave>> ApplyLeave([FromBody] Leave leave)
        {
            leave.EmployeeId = GetUserId();
            leave.Status = "Pending";  // Default status when applying

            await _leaveService.AddLeave(leave);
            return CreatedAtAction(nameof(GetLeaveById), new { id = leave.LeaveId }, leave);
        }

        // PUT: api/leave/{id} - Employees can cancel their own leave (only if pending)
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CancelLeave(int id)
        {
            var leave = await _leaveService.GetLeaveById(id);
            if (leave == null)
                return NotFound(new { message = "Leave request not found" });

            if (leave.EmployeeId != GetUserId())
                return Unauthorized(new { message = "You can only cancel your own leave requests" });

            if (leave.Status != "Pending")
                return BadRequest(new { message = "Only pending leave requests can be cancelled" });

            leave.Status = "Cancelled";
            await _leaveService.UpdateLeave(leave);
            return NoContent();
        }

        // PUT: api/leave/approve/{id} - Admin approves a leave request
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            var leave = await _leaveService.GetLeaveById(id);
            if (leave == null)
                return NotFound(new { message = "Leave request not found" });

            leave.Status = "Approved";
            await _leaveService.UpdateLeave(leave);
            return NoContent();
        }

        // PUT: api/leave/reject/{id} - Admin rejects a leave request
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            var leave = await _leaveService.GetLeaveById(id);
            if (leave == null)
                return NotFound(new { message = "Leave request not found" });

            leave.Status = "Rejected";
            await _leaveService.UpdateLeave(leave);
            return NoContent();
        }

        // DELETE: api/leave/{id} - Admin can delete leave requests
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var leave = await _leaveService.GetLeaveById(id);
            if (leave == null)
                return NotFound(new { message = "Leave request not found" });

            await _leaveService.DeleteLeave(id);
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
