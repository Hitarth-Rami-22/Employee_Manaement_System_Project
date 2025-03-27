using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Repository;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;

namespace EmployeeManagementSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        public async Task<AdminDTO> GetAdminByEmail(string email)
        {
            var admin = await _adminRepository.GetAdminByEmail(email);
            return _mapper.Map<AdminDTO>(admin);
        }

        public async Task AddAdmin(AdminCreateDTO adminCreateDTO)
        {
            var admin = _mapper.Map<Admin>(adminCreateDTO);

            // Hash password before storing
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminCreateDTO.Password);

            await _adminRepository.AddAdmin(admin);
        }
        public async Task<AdminDTO> VerifyAdminCredentials(string email, string password)
        {
            var admin = await _adminRepository.GetAdminByEmail(email);
            if (admin == null || string.IsNullOrEmpty(admin.PasswordHash) || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                return null;

            return _mapper.Map<AdminDTO>(admin);
        }

        public async Task<AdminDTO> GetAdminById(int id)
        {
            var admin = await _adminRepository.GetAdminById(id);
            return _mapper.Map<AdminDTO>(admin);
        }
    }
}
