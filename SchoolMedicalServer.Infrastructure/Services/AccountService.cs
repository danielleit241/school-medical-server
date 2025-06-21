using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

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

            var parentPhones = students
                .Where(s => !string.IsNullOrEmpty(s.ParentPhoneNumber))
                .Select(s => s.ParentPhoneNumber!)
                .Distinct()
                .ToList();

            var existingUsers = await userRepository.GetUsersByPhoneNumbersAsync(parentPhones);

            var studentsByParentPhone = students
                .Where(s => !string.IsNullOrEmpty(s.ParentPhoneNumber))
                .GroupBy(s => s.ParentPhoneNumber!)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<AccountResponse> accounts = [];

            foreach (var parentPhone in parentPhones)
            {
                if (!studentsByParentPhone.TryGetValue(parentPhone, out var studentsWithSameParent))
                    continue;

                if (existingUsers.TryGetValue(parentPhone, out var existingUser))
                {
                    foreach (var student in studentsWithSameParent)
                    {
                        student.UserId = existingUser.UserId;
                        studentRepository.UpdateStudent(student);
                    }
                    continue;
                }

                var role = await userRepository.GetRoleByNameAsync(configuration["DefaultAccountCreate:RoleName"]!);
                if (role == null)
                    return null!;

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    PhoneNumber = parentPhone,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, defaultPassword),
                    RoleId = role.RoleId,
                    EmailAddress = studentsWithSameParent.First().ParentEmailAddress,
                    Status = true,
                    CreatedAt = DateTime.UtcNow,
                };

                foreach (var student in studentsWithSameParent)
                {
                    student.UserId = user.UserId;
                    studentRepository.UpdateStudent(student);

                    accounts.Add(new AccountResponse
                    {
                        Id = user.UserId,
                        FullName = "Parent of " + student.FullName,
                        EmailAddress = user.EmailAddress!,
                        PhoneNumber = user.PhoneNumber,
                        Password = defaultPassword
                    });
                }

                await userRepository.AddUserAsync(user);
                existingUsers[parentPhone] = user;
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
                CreatedAt = DateTime.UtcNow,
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