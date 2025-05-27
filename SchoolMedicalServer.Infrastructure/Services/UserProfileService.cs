using SchoolMedicalServer.Abstractions.Dtos.UserProfile;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserProfileService(SchoolMedicalManagementContext context) : IUserProfileService
    {
        public async Task<UserProfileDto?> GetUserProfileByIdAsync(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            var response = new UserProfileDto
            {
                FullName = user.FullName!,
                Email = user.EmailAddress!,
                DateOfBirth = user.DayOfBirth,
                AvatarUrl = user.AvatarUrl
            };

            return response;
        }

        public async Task<UserProfileDto?> UpdateUserProfileAsync(Guid userId, UserProfileDto dto)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            user.FullName = dto.FullName;
            user.EmailAddress = dto.Email;
            user.DayOfBirth = dto.DateOfBirth;
            user.AvatarUrl = dto.AvatarUrl;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            var response = new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.EmailAddress,
                DateOfBirth = user.DayOfBirth,
                AvatarUrl = user.AvatarUrl
            };

            return response;
        }
    }
}

