using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using EmployeeManagementSystem.Services;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;


namespace EmployeeManagementSystem.Authentication
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenExpirationSeconds;
        private readonly int _refreshTokenExpirationDays;
        private readonly ILogger<JwtAuthenticationService> _logger;


        public JwtAuthenticationService(IConfiguration configuration, ILogger<JwtAuthenticationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            _issuer = _configuration["Jwt:Issuer"];
            _audience = _configuration["Jwt:Audience"];
            _tokenExpirationSeconds = int.Parse(_configuration["Jwt:ExpirationHours"] ?? "2");
            _refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            _logger = logger;
        }

        public string GenerateToken(string email, string role)
        {
            _logger.LogInformation($"Generating token for user: {email}, Role: {role}");

            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                 _issuer,
                 _audience,
                 claims,
                 expires: DateTime.UtcNow.AddSeconds(int.Parse(_configuration["Jwt:ExpirationSeconds"] ?? "3600")), // ✅ Uses correct expiration time
                 signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public string GenerateRefreshToken()
        // {
        //    var randomBytes = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(randomBytes);
        //    }
        //    return Convert.ToBase64String(randomBytes);
        //}
    
    }
}
