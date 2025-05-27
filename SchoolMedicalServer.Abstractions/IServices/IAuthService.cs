using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAuthService
    {
        Task<User?> ChangePasswordAsync(ChangePasswordRequest request);
        Task<TokensResponse?> LoginAsync(LoginRequest request);
        Task<TokensResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
