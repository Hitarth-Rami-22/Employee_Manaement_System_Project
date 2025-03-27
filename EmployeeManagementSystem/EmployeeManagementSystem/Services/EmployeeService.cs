
using System.Text;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using EmployeeManagementSystem.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            _logger.LogInformation("Fetching all employees.");
            var employees = await _employeeRepository.GetAllEmployees();
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO> GetEmployeeById(int id)
        {
            _logger.LogInformation($"Fetching Employee with ID: {id}");
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                _logger.LogWarning($"Employee with ID {id} not found.");
                throw new KeyNotFoundException("Employee not found");
            }
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetEmployeeByEmail(string email)
        {
            _logger.LogInformation($"Fetching Employee with Email: {email}");
            var employee = await _employeeRepository.GetEmployeeByEmail(email);
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> VerifyEmployeeCredentials(string email, string password)
        {
            _logger.LogInformation($"Verifying credentials for Email: {email}");
            var employee = await _employeeRepository.GetEmployeeByEmail(email);
            if (employee == null || string.IsNullOrEmpty(employee.PasswordHash) || !BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash))
            {
                _logger.LogWarning("Invalid credentials.");
                return null;
            }
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task AddEmployee(EmployeeCreateDTO employeeCreateDTO)
        {
            _logger.LogInformation("Adding a new employee.");
            var employee = _mapper.Map<Employee>(employeeCreateDTO);
            employee.PasswordHash = HashPassword(employeeCreateDTO.Password);
            await _employeeRepository.AddEmployee(employee);
        }

        public async Task UpdateEmployee(int employeeId, EmployeeUpdateDTO employeeUpdateDTO)
        {
            _logger.LogInformation($"Updating Employee with ID: {employeeId}");
            var existingEmployee = await _employeeRepository.GetEmployeeById(employeeId);
            if (existingEmployee == null)
            {
                _logger.LogWarning($"Employee with ID {employeeId} not found.");
                return;
            }
            _mapper.Map(employeeUpdateDTO, existingEmployee);
            await _employeeRepository.UpdateEmployee(existingEmployee);
        }

        public async Task DeleteEmployee(int id)
        {
            _logger.LogInformation($"Deleting Employee with ID: {id}");
            await _employeeRepository.DeleteEmployee(id);
        }

        public async Task UpdateEmployeeProfile(int employeeId, string phone, string address, string techStack)
        {
            _logger.LogInformation($"Updating profile for Employee ID: {employeeId}");
            await _employeeRepository.UpdateEmployeeProfile(employeeId, phone, address, techStack);
        }

        public async Task UpdateEmployeePassword(int employeeId, string newPassword)
        {
            _logger.LogInformation($"Updating password for Employee ID: {employeeId}");
            string hashedPassword = HashPassword(newPassword);
            await _employeeRepository.UpdateEmployeePassword(employeeId, hashedPassword);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
