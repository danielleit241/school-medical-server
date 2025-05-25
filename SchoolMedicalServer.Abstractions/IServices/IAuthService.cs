using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<User?> RegisterAsync(UserDto request);
    }
}
