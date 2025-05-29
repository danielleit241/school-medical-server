using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AccountService(SchoolMedicalManagementContext context, IConfiguration configuration) : IAccountService
    {
        public async Task<List<AccountDto>> BatchCreateParentsAsync()
        {
            string? defaultPassword = configuration["DefaultAccountCreate:Password"];
            if (string.IsNullOrEmpty(defaultPassword))
                return [];

            var students = await context.Students
                .Where(s => s.ParentPhoneNumber != null)
                .ToListAsync();

            var parentPhones = students
                .Select(s => s.ParentPhoneNumber)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToList();

            var existingUsers = await context.Users
                .Where(u => parentPhones.Contains(u.PhoneNumber))
                .ToDictionaryAsync(u => u.PhoneNumber);

            List<AccountDto> accounts = [];

            foreach (var student in students)
            {
                if (string.IsNullOrEmpty(student.ParentPhoneNumber))
                    continue;

                if (existingUsers.TryGetValue(student.ParentPhoneNumber, out var existingUser))
                {
                    student.UserId = existingUser.UserId;
                    context.Students.Update(student);
                    continue;
                }
                var role = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == configuration["DefaultAccountCreate:RoleName"]);

                if (role == null)
                    return null;

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    PhoneNumber = student.ParentPhoneNumber!,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, defaultPassword),
                    RoleId = role.RoleId,
                    EmailAddress = student.ParentEmailAddress,
                    Status = true
                };

                student.UserId = user.UserId;

                accounts.Add(new AccountDto
                {
                    Id = user.UserId,
                    FullName = student.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Password = defaultPassword
                });

                context.Users.Add(user);
                context.Students.Update(student);

                // Add to local dictionary to prevent duplicate users in this batch
                existingUsers[user.PhoneNumber] = user;
            }

            await context.SaveChangesAsync();
            return accounts;
        }

        public async Task<AccountDto?> RegisterStaffAsync(RegisterStaffRequest request)
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
            if (string.IsNullOrEmpty(request.RoleName) || string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email))
            {
                return null;
            }

            var role = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.RoleName);

            if (role == null)
            {
                return null;
            }

            user.PhoneNumber = request.PhoneNumber;
            user.FullName = request.FullName;
            user.EmailAddress = request.Email;
            user.Status = true;
            user.RoleId = role.RoleId;

            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var account = new AccountDto
            {
                Id = user.UserId,
                FullName = request.FullName,
                PhoneNumber = user.PhoneNumber,
                Password = request.Password
            };

            return account;
        }
    }
}
