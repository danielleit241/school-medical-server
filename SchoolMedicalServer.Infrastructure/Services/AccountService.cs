using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AccountService(
        IBaseRepository baseRepository,
        IUserRepository userRepository,
        IStudentRepository studentRepository,
        IConfiguration configuration) : IAccountService
    {
        public async Task<List<AccountResponse>> BatchCreateParentsAsync()
        {
            string? defaultPassword = configuration["DefaultAccountCreate:Password"];
            if (string.IsNullOrEmpty(defaultPassword))
                return [];

            var students = await studentRepository.GetStudentsWithParentPhoneAsync();

            var parentPhones = await studentRepository.GetParentsPhoneNumber();

            var existingUsers = await userRepository.GetUsersByPhoneNumbersAsync(parentPhones);

            List<AccountResponse> accounts = [];

            foreach (var student in students)
            {
                if (string.IsNullOrEmpty(student.ParentPhoneNumber))
                    continue;

                if (existingUsers.TryGetValue(student.ParentPhoneNumber, out var existingUser))
                {
                    student.UserId = existingUser.UserId;
                    studentRepository.UpdateStudent(student);
                    continue;
                }

                var role = await userRepository.GetRoleByNameAsync(configuration["DefaultAccountCreate:RoleName"]!);
                if (role == null)
                    return null!;

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    PhoneNumber = student.ParentPhoneNumber!,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, defaultPassword),
                    RoleId = role.RoleId,
                    EmailAddress = student.ParentEmailAddress,
                    Status = true,
                    CreateAt = DateTime.UtcNow,
                };

                student.UserId = user.UserId;

                accounts.Add(new AccountResponse
                {
                    Id = user.UserId,
                    FullName = "Parent of " + student.FullName,
                    EmailAddress = user.EmailAddress!,
                    PhoneNumber = user.PhoneNumber,
                    Password = defaultPassword
                });

                await userRepository.AddUserAsync(user);
                studentRepository.UpdateStudent(student);

                existingUsers[user.PhoneNumber] = user;
            }

            await baseRepository.SaveChangesAsync();
            return accounts;
        }

        public async Task<AccountResponse?> RegisterStaffAsync(RegisterStaffRequest request)
        {
            if (await userRepository.UserExistsByPhoneNumberAsync(request.PhoneNumber))
            {
                return null;
            }

            if (string.IsNullOrEmpty(request.RoleName) || string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email))
            {
                return null;
            }

            var role = await userRepository.GetRoleByNameAsync(request.RoleName);

            if (role == null)
            {
                return null;
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName,
                EmailAddress = request.Email,
                Status = true,
                RoleId = role.RoleId,
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, request.Password),
                CreateAt = DateTime.UtcNow,
            };

            await userRepository.AddUserAsync(user);
            await baseRepository.SaveChangesAsync();

            return new AccountResponse
            {
                Id = user.UserId,
                FullName = request.FullName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                Password = request.Password
            };
        }
    }
}