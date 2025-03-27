using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> GetEmployeeByEmail(string email);
        Task AddEmployee(Employee employee);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);

        Task UpdateEmployeeProfile(int employeeId, string phone, string address, string techStack);
        Task UpdateEmployeePassword(int employeeId, string hashedPassword);
    }
}
