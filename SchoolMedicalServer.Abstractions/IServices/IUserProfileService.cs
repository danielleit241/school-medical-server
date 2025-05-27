using SchoolMedicalServer.Abstractions.Dtos.UserProfile;


namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse?> GetUserProfileByIdAsync(Guid userId);
        Task<UserProfileResponse?> UpdateUserProfileAsync(Guid userId, UserProfileRequest dto);
    }
}
