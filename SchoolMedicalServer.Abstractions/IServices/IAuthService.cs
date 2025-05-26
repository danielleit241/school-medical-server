using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAuthService
    {
        Task<Entities.User?> ChangePasswordAsync(ChangePasswordRequest request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequest request);
        Task<Entities.User?> RegisterAsync(UserDto request);
    }
}
