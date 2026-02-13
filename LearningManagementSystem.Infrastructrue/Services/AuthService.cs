using LearningManagementSystem.Core.DTOs.Auth;
using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;
using LearningManagementSystem.Core.Models.Entities;
using LearningManagementSystem.Core.Models.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearningManagementSystem.Infrastructrue.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<ResultDto<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto)
        {

            var existingUser = await _unitOfWork.Users
        .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
                return ResultDto<AuthResponseDto>.FailureResult("Email already registered");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            var response = new AuthResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(
              int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60"))
            };

            return ResultDto<AuthResponseDto>.SuccessResult(response, "Registration successful");
        }

        public async Task<ResultDto<AuthResponseDto>> LoginAsync(LoginRequestDto dto)
        {

            var user = await _unitOfWork.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return ResultDto<AuthResponseDto>.FailureResult("Invalid credentials");

            if (!user.IsActive)
                return ResultDto<AuthResponseDto>.FailureResult("Account is deactivated");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return ResultDto<AuthResponseDto>.FailureResult("Invalid credentials");

            var token = GenerateJwtToken(user);

            var response = new AuthResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60"))
            };

            return ResultDto<AuthResponseDto>.SuccessResult(response, "Login successful");
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"] ?? "");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"] ?? "");

            var claims = new[]
               {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60")),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

