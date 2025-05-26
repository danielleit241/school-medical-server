using SchoolMedicalServer.Abstractions.Dtos.Profile;
using SchoolMedicalServer.Abstractions.Entities;


namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserProfileService
    {

        Task<User?> GetUserProfileByIdAsync(Guid userId);
        Task<User?> UpdateUserProfileAsync(Guid userId, UserProfileDTO dto);
    }
}
