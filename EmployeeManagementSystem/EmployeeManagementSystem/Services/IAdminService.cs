using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using System.Threading.Tasks;


namespace EmployeeManagementSystem.Services
{
        public interface IAdminService
        {
        Task<AdminDTO> GetAdminByEmail(string email);
        Task AddAdmin(AdminCreateDTO adminCreateDTO);
        Task<AdminDTO> GetAdminById(int id);
        Task<AdminDTO> VerifyAdminCredentials(string email, string password);


    }
}