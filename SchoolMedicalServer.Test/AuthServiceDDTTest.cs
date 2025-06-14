using Microsoft.AspNetCore.Identity;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Infrastructure;
namespace SchoolMedicalServer.Test
{
    public class AuthServiceDDTTest : TestExtensions
    {
        public static readonly List<(string Phone, string Password)> Users = [
            ("1234567890", "YourStr0ngPassw0rd!"),
            ("0987654321", "AnotherStr0ngPassw0rd!"),
            ("5555555555", "YetAnotherStr0ngPassw0rd!")
        ];

        private static void SeedRolesAndUsers(SchoolMedicalManagementContext context, List<(string Phone, string Password)> users)
        {
            var roles = new[]
            {
                new Role {RoleName = "admin" },
                new Role {RoleName = "manager" },
                new Role {RoleName = "nurse" },
                new Role {RoleName = "parent" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();
            var adminRole = context.Roles.First(r => r.RoleName == "admin");
            var userEntities = users.Select(u => new User
            {
                UserId = Guid.NewGuid(),
                PhoneNumber = u.Phone,
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, u.Password),
                RoleId = adminRole.RoleId,
                Status = true
            });
            context.Users.AddRange(userEntities);
            context.SaveChanges();
        }

        public static IEnumerable<object[]> ValidLoginData() => Users.ToList().Select(u => new object[] { u.Phone, u.Password });
        public static IEnumerable<object[]> InvalidLoginData() => [
            ["1234567890", "WrongPassword!"],
            ["0000000000", "YourStr0ngPassw0rd!"],
            ["0000000000", "WrongPassword!"]
        ];


        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var loginRequest = new UserLoginRequest
            {
                PhoneNumber = phoneNumber,
                Password = password
            };
            var result = await authService.LoginAsync(loginRequest);
            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);
            Assert.NotEmpty(result.RefreshToken);
        }


        [Theory]
        [MemberData(nameof(InvalidLoginData))]
        public async Task AuthenticateUser_ShouldReturnNull_WhenCredentialsAreInvalid(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var loginRequest = new UserLoginRequest
            {
                PhoneNumber = phoneNumber,
                Password = password
            };
            var result = await authService.LoginAsync(loginRequest);
            Assert.Null(result);
        }
    }
}
