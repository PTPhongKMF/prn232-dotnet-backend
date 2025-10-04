using MathslideLearning.Data.Entities;
using MathslideLearning.Models.AccountsDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MathslideLearning.Business.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserResponseDto> UpdateUserAsync(int userId, UpdateUserRequestDto request);
        Task<bool> SoftDeleteUserAsync(int userId);
        Task<UserResponseDto> AdminUpdateUserAsync(int userId, AdminUpdateUserRequestDto request);

        // --- New Methods ---
        Task<UserResponseDto> GetUserProfileAsync(int userId);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    }
}

