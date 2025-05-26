using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AccountServices(SchoolMedicalManagementContext context) : IAccountServices
    {
        public async Task<User?> RegisterStaffAsync(RegisterRequestDto request)
        {
            if (await context.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber))
            {
                return null;
            }

            var user = new User();
            if (user.UserId == Guid.Empty)
            {
                user.UserId = Guid.NewGuid();
            }
            if (string.IsNullOrEmpty(request.RoleId) || string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email))
            {
                return null;
            }

            if (int.TryParse(request.RoleId, out var roleId))
            {
                user.RoleId = roleId;
            }

            user.PhoneNumber = request.PhoneNumber;
            user.FullName = request.FullName;
            user.EmailAddress = request.Email;

            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}
