using AutoMapper;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;



namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IAdminService _adminService;
        private readonly IEmployeeService _employeeService;
        private readonly ITimesheetService _timesheetService;
        private readonly ILeaveService _leaveService;

        public AdminController(IAdminService adminService,
            IEmployeeService employeeService,
            ITimesheetService timesheetService,
            ILeaveService leaveService,
            IMapper mapper)
        {
            _adminService = adminService;
            _employeeService = employeeService;
            _timesheetService = timesheetService;
            _leaveService = leaveService;
            _mapper = mapper;
        }

        // POST: api/admin (Create a new admin)
        [HttpPost]
        public async Task<IActionResult> AddAdmin([FromBody] AdminCreateDTO adminCreateDTO)
        {
            if (adminCreateDTO == null)
                return BadRequest(new { message = "Invalid admin data" });

            await _adminService.AddAdmin(adminCreateDTO);

            var createdAdmin = await _adminService.GetAdminByEmail(adminCreateDTO.Email);

            return CreatedAtAction(nameof(GetAdminById), new { id = createdAdmin.AdminId }, createdAdmin);
        }

        //  GET: api/admin/employees
        [HttpGet("employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployees();
            return Ok(employees);
        }

        // GET: api/admin/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDTO>> GetAdminById(int id)
        {
            var admin = await _adminService.GetAdminById(id);
            if (admin == null)
                return NotFound(new { message = "Admin not found" });

            return Ok(_mapper.Map<AdminDTO>(admin));
        }

        // GET: api/admin/employees/{id}
        [HttpGet("employees/{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(_mapper.Map<EmployeeDTO>(employee));
        }

        // PUT: api/admin/employees/{id}
        [HttpPut("employees/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO updateDTO)
        {
            var existingEmployee = await _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound(new { message = "Employee not found" });

            await _employeeService.UpdateEmployee(id, updateDTO);

            return NoContent();
        }

        // DELETE: api/admin/employees/{id}
        [HttpDelete("employees/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var existingEmployee = await _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound(new { message = "Employee not found" });

            await _employeeService.DeleteEmployee(id);
            return NoContent();
        }

        //  Employee Work Hour Reports (weekly/monthly)
        [HttpGet("reports/workhours")]
        public async Task<IActionResult> GetEmployeeWorkHours([FromQuery] string period)
        {
            if (string.IsNullOrEmpty(period))
                return BadRequest(new { message = "Period is required (weekly or monthly)." });

            var report = await _timesheetService.GetEmployeeWorkHours(period);
            return Ok(report);
        }


        //FUNCTIONALITY: Export Timesheets to Excel
        [HttpGet("export/timesheets")]
        public async Task<IActionResult> ExportTimesheets()
        {
            var fileBytes = await _timesheetService.ExportTimesheetsToExcel();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Timesheets.xlsx");
        }

        //FUNCTIONALITY: Export Leave Requests to Excel
        [HttpGet("export/leaves")]
        public async Task<IActionResult> ExportLeaves()
        {
            var fileBytes = await _leaveService.ExportLeavesToExcel();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Leaves.xlsx");
        }
    }
}
