using LearningManagementSystem.Core.DTOs.Common;
using LearningManagementSystem.Core.DTOs.User;
using LearningManagementSystem.Core.Interfaces;
using LearningManagementSystem.Core.Interfaces.Services;

namespace LearningManagementSystem.Infrastructrue.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<UserResponseDto>> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ResultDto<UserResponseDto>.FailureResult("User not found");

            var dto = new UserResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return ResultDto<UserResponseDto>.SuccessResult(dto);
        }

        public async Task<ResultDto<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var userDtos = users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });

            return ResultDto<IEnumerable<UserResponseDto>>.SuccessResult(userDtos);
        }

        public async Task<ResultDto<bool>> DeactivateUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ResultDto<bool>.FailureResult("User not found");

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "User deactivated successfully");
        }

        public async Task<ResultDto<bool>> ActivateUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ResultDto<bool>.FailureResult("User not found");

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return ResultDto<bool>.SuccessResult(true, "User activated successfully");
        }
    }
}
