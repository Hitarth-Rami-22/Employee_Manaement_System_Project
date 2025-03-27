namespace EmployeeManagementSystem.Services
{
    public interface IJwtAuthenticationService
    {
        string GenerateToken(string email, string role);
    }
}
