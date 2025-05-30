using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAuthService
    {
        Task<User?> ChangePasswordAsync(ChangePasswordRequest request);
        Task<string> GetOtpAsync(SendOtpRequest request);
        Task<TokensResponse?> LoginAsync(LoginRequest request);
        Task<TokensResponse?> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> VerifyOtpAsync(string otp);
    }
}
