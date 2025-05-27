using SchoolMedicalServer.Abstractions.Dtos.User;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserService
    {
        Task<List<UserDto>?> GetAllAsync();
        Task<UserDto?> GetUserAsync(Guid userId);
        Task<bool> UpdateUserAsync(Guid userid, UserDto request);
    }
}
