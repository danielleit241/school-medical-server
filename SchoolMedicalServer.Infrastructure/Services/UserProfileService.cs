using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserProfileService(SchoolMedicalManagementContext context) : IUserProfileService
    {
        public async Task<UserProfileResponse?> GetUserProfileByIdAsync(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            var response = new UserProfileResponse
            {
                FullName = user.FullName!,
                EmailAddress = user.EmailAddress!,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DayOfBirth,
                AvatarUrl = user.AvatarUrl,
                Address = user.Address ?? string.Empty
            };

            return response;
        }

        public async Task<UserProfileResponse?> UpdateUserProfileAsync(Guid userId, UserProfileRequest dto)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            user.FullName = dto.FullName;
            user.EmailAddress = dto.EmailAddress;
            user.DayOfBirth = dto.DateOfBirth;
            user.AvatarUrl = dto.AvatarUrl;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            var response = new UserProfileResponse
            {
                FullName = user.FullName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DayOfBirth,
                AvatarUrl = user.AvatarUrl,
                Address = user.Address ?? string.Empty
            };

            return response;
        }
    }
}

