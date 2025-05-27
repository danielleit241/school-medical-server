using SchoolMedicalServer.Abstractions.Dtos.UserProfile;


namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetUserProfileByIdAsync(Guid userId);
        Task<UserProfileDto?> UpdateUserProfileAsync(Guid userId, UserProfileDto dto);
    }
}
