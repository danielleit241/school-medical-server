using SchoolMedicalServer.Abstractions.Dtos.Profile;
using SchoolMedicalServer.Abstractions.Entities;


namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserProfileService
    {

        Task<UserProfileDTO?> GetUserProfileByIdAsync(Guid userId);
        Task<UserProfileDTO?> UpdateUserProfileAsync(Guid userId, UserProfileDTO dto);
    }
}
