using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Repository;
using Microsoft.Extensions.Logging;


namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;

        }

        //  GET: api/employee (Fetch all employees)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            try
            {
                return Ok(await _employeeService.GetAllEmployees());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employees.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }
        //  GET: api/employee/{id} (Fetch employee by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            try
            {
                return Ok(await _employeeService.GetEmployeeById(id));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Employee with ID {id} not found.");
                return NotFound(new ErrorResponseDTO { Message = "Employee not found", Details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }

        // POST: api/employee (Create Employee)
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> AddEmployee([FromBody] EmployeeCreateDTO employeeCreateDTO)
        {
            try
            {
                if (employeeCreateDTO == null)
                    return BadRequest(new ErrorResponseDTO { Message = "Invalid employee data" });

                await _employeeService.AddEmployee(employeeCreateDTO);
                var createdEmployee = await _employeeService.GetEmployeeByEmail(employeeCreateDTO.Email);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.EmployeeId }, createdEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }

        //  PUT: api/employee/{id} (Update Employee)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO updateDTO)
        {
            try
            {
                var existingEmployee = await _employeeService.GetEmployeeById(id);
                if (existingEmployee == null)
                    return NotFound(new ErrorResponseDTO { Message = "Employee not found" });

                await _employeeService.UpdateEmployee(id, updateDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }

        //  DELETE: api/employee/{id} (Delete Employee)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var existingEmployee = await _employeeService.GetEmployeeById(id);
                if (existingEmployee == null)
                    return NotFound(new ErrorResponseDTO { Message = "Employee not found" });

                await _employeeService.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }

        //  PUT: api/employee/profile (Update Profile)
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                int employeeId = GetUserId();
                await _employeeService.UpdateEmployeeProfile(employeeId, request.Phone, request.Address, request.TechStack);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }

        //  PUT: api/employee/password (Update Password)
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            try
            {
                int employeeId = GetUserId();
                await _employeeService.UpdateEmployeePassword(employeeId, request.NewPassword);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password.");
                return StatusCode(500, new ErrorResponseDTO { Message = "Internal server error", Details = ex.Message });
            }
        }
        //  Helper method to extract user ID from claims
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        


        // Request model for updating profile
        public class UpdateProfileRequest
        {
            public string Phone { get; set; }
            public string Address { get; set; }
            public string TechStack { get; set; }
        }

        //  Request model for updating password
        public class UpdatePasswordRequest
        {
            public string NewPassword { get; set; }
        }
    }
}




//using EmployeeManagementSystem.Models;
//using EmployeeManagementSystem.Services;
//using Microsoft.AspNetCore.Authorization; //  Added for authorization
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using AutoMapper;
//using EmployeeManagementSystem.DTOs;
//using EmployeeManagementSystem.Repository;

//namespace EmployeeManagementSystem.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(Roles = "Employee")] //  Added authorization to restrict access to employees
//    public class EmployeeController : ControllerBase
//    {

//        private readonly IEmployeeService _employeeService;
//        private readonly IMapper _mapper;

//        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
//        {
//            _employeeService = employeeService;
//            _mapper = mapper;
//        }




//        // GET: api/employee
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
//        {
//            var employees = await _employeeService.GetAllEmployees();
//            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
//            return Ok(employeeDTOs);
//        }


//        //  GET: api/employee/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
//        {
//            var employee = await _employeeService.GetEmployeeById(id);
//            if (employee == null)
//                return NotFound(new { message = "Employee not found" });

//            return Ok(_mapper.Map<EmployeeDTO>(employee));
//        }

//        //  POST: api/employee (Create Employee)
//        [HttpPost]
//        public async Task<ActionResult<EmployeeDTO>> AddEmployee([FromBody] EmployeeCreateDTO employeeCreateDTO)
//        {
//            if (employeeCreateDTO == null)
//                return BadRequest(new { message = "Invalid employee data" });

//            // Call service layer to handle employee creation
//            await _employeeService.AddEmployee(employeeCreateDTO);

//            // Retrieve the created employee and return DTO
//            var createdEmployee = await _employeeService.GetEmployeeByEmail(employeeCreateDTO.Email);

//            // Return the created employee DTO
//            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.EmployeeId }, createdEmployee);
//        }

//        // PUT: api/employee/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO updateDTO)
//        {
//            //if (id != employee.EmployeeId)
//            //    return BadRequest(new { message = "ID mismatch" });

//            var existingEmployee = await _employeeService.GetEmployeeById(id);
//            if (existingEmployee == null)
//                return NotFound(new { message = "Employee not found" });

//            // Update allowed fields
//            _mapper.Map(updateDTO, existingEmployee);
//            await _employeeService.UpdateEmployee(id, updateDTO);

//            //await _employeeService.UpdateEmployee(employee);
//            return NoContent();
//        }

//        // DELETE: api/employee/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteEmployee(int id)
//        {
//            var existingEmployee = await _employeeService.GetEmployeeById(id);
//            if (existingEmployee == null)
//                return NotFound(new { message = "Employee not found" });

//            await _employeeService.DeleteEmployee(id);
//            return NoContent();
//        }

//        // API: PUT api/employee/profile - Update Profile
//        [HttpPut("profile")]
//        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
//        {
//            int employeeId = GetUserId();
//            await _employeeService.UpdateEmployeeProfile(employeeId, request.Phone, request.Address, request.TechStack);
//            return NoContent();
//        }

//        // API: PUT api/employee/password - Update Password
//        [HttpPut("password")]
//        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
//        {
//            int employeeId = GetUserId();
//            await _employeeService.UpdateEmployeePassword(employeeId, request.NewPassword);
//            return NoContent();
//        }

//        // Helper method to extract user ID from claims
//        private int GetUserId()
//        {
//            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
//        }
//    }

//    //   Request model for updating profile
//    public class UpdateProfileRequest
//    {
//        public string Phone { get; set; }
//        public string Address { get; set; }
//        public string TechStack { get; set; }
//    }

//    //  Request model for updating password
//    public class UpdatePasswordRequest
//    {
//        public string NewPassword { get; set; }
//    }
//}