using EmployeeManagementSystem.Models;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Repository
{
     public interface IAdminRepository
     {
        Task<Admin> GetAdminByEmail(string email);
        Task<Admin> GetAdminById(int id); 
        Task AddAdmin(Admin admin);
     }
}