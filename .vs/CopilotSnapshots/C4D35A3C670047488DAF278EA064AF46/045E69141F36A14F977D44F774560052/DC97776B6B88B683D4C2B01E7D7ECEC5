using LearningManagementSystem.Core.DTOs.Auth;
using LearningManagementSystem.Core.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResultDto<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto);
        Task<ResultDto<AuthResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<bool> ValidateTokenAsync(string token);
    }
}
