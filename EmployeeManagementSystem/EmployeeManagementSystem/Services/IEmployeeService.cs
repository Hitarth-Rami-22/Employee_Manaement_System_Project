using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.DTOs;

namespace EmployeeManagementSystem.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployees();
        Task<EmployeeDTO> GetEmployeeById(int id);
        Task<EmployeeDTO> GetEmployeeByEmail(string email);
        Task<EmployeeDTO> VerifyEmployeeCredentials(string email, string password);

        Task AddEmployee(EmployeeCreateDTO employeeCreateDTO);
        Task UpdateEmployee(int employeeId, EmployeeUpdateDTO employeeUpdateDTO);
        Task DeleteEmployee(int id);

        Task UpdateEmployeeProfile(int employeeId, string phone, string address, string techStack);
        Task UpdateEmployeePassword(int employeeId, string newPassword);


    }
}
