using LearningManagementSystem.Core.DTOs.User;
using LearningManagementSystem.Core.DTOs.Common;

namespace LearningManagementSystem.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResultDto<UserResponseDto>> GetUserByIdAsync(int userId);
        Task<ResultDto<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<ResultDto<bool>> DeactivateUserAsync(int userId);
        Task<ResultDto<bool>> ActivateUserAsync(int userId);
    }
}
