using SchoolMedicalServer.Abstractions.Dtos.User;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse?> GetUserProfileByIdAsync(Guid userId);
        Task<UserProfileResponse?> UpdateUserProfileAsync(Guid userId, UserProfileRequest dto);
        Task<string?> UpdateUserProfileImageAsync(Guid userId, UserProfileRequest dto);
    }
}
