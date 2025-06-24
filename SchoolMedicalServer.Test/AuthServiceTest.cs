using Microsoft.AspNetCore.Identity;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Infrastructure;

namespace SchoolMedicalServer.Test
{
    public class AuthServiceTest : TestExtensions
    {

        private void SeedRolesAndUser(SchoolMedicalManagementContext context)
        {
            var roles = new[]
            {
                new Role {RoleId = 1, RoleName = "admin" },
                new Role {RoleId = 3, RoleName = "manager" },
                new Role {RoleId = 2, RoleName = "nurse" },
                new Role {RoleId = 4, RoleName = "parent" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();


            var adminRole = context.Roles.First(r => r.RoleName == "admin");
            var user = new User
            {
                UserId = Guid.NewGuid(),
                PhoneNumber = "1234567890",
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, "YourStr0ngPassw0rd!"),
                RoleId = adminRole.RoleId,
                Status = true
            };
            context.Users.Add(user);
            context.SaveChanges();

        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var context = CreateContext();
            SeedRolesAndUser(context);

            var authService = CreateAuthService(context);
            var loginRequest = new UserLoginRequest
            {
                PhoneNumber = "1234567890",
                Password = "YourStr0ngPassw0rd!"
            };

            var result = await authService.LoginAsync(loginRequest);

            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);
            Assert.NotEmpty(result.RefreshToken);
        }

        [Fact]
        public async Task AuthenticateUser_ShouldReturnNull_WhenPhoneNumberIsInvalid()
        {
            var context = CreateContext();
            SeedRolesAndUser(context);

            var authService = CreateAuthService(context);
            var loginRequest = new UserLoginRequest
            {
                PhoneNumber = "0000000000",
                Password = "YourStr0ngPassw0rd!"
            };

            var result = await authService.LoginAsync(loginRequest);

            Assert.Null(result);
        }

    }
}