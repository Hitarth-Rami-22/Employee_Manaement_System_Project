using EmployeeManagementSystem.Authentication;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;


namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAdminService _adminService;
        private readonly JwtAuthenticationService _jwtAuthenticationService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IEmployeeService employeeService, IAdminService adminService, JwtAuthenticationService jwtAuthenticationService, ILogger<AuthController> logger)
        {
            _employeeService = employeeService;
            _adminService = adminService;
            _jwtAuthenticationService = jwtAuthenticationService;
            _logger = logger;
        }

        // Employee Login Endpoint
        [HttpPost("employee/login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginDTO loginRequest)
        {
            var employee = await _employeeService.VerifyEmployeeCredentials(loginRequest.Email, loginRequest.Password);
            if (employee == null)
            {
                _logger.LogWarning($"Failed login attempt for email: {loginRequest.Email}");
                return Unauthorized(new { message = "Invalid employee credentials" });
            }
            // Generate JWT Token
            var token = _jwtAuthenticationService.GenerateToken(employee.Email, "Employee");
            return Ok(new { token });
        }


        // Admin Login Endpoint
        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginDTO loginRequest)
        {
            var admin = await _adminService.VerifyAdminCredentials(loginRequest.Email, loginRequest.Password);
            if (admin == null)
            {
                _logger.LogWarning($"Failed admin login attempt for email: {loginRequest.Email}");
                return Unauthorized(new { message = "Invalid admin credentials" });
            }
            var token = _jwtAuthenticationService.GenerateToken(admin.Email, "Admin");
            return Ok(new { token });
        }


    }
}
