using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
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
                new Role {RoleId = 1, RoleName = "admin" },
                new Role {RoleId = 3, RoleName = "manager" },
                new Role {RoleId = 2, RoleName = "nurse" },
                new Role {RoleId = 4, RoleName = "parent" }
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();
            var adminRole = context.Roles.First(r => r.RoleName == "admin");
            var userEntities = users.Select(u => new User
            {
                UserId = Guid.NewGuid(),
                PhoneNumber = u.Phone,
                FullName = $"User {u.Phone}",
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
            ["9817232134", "YourStr0ngPassw0rd!"],
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(result.AccessToken);

            jwtToken.Should().NotBeNull();
            jwtToken.Issuer.Should().Be("TestIssuer");
            jwtToken.Audiences.Should().Contain("TestAudience");

            var user = context.Users.First(u => u.PhoneNumber == phoneNumber);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            userIdClaim.Should().NotBeNull();
            userIdClaim!.Value.Should().Be(user.UserId.ToString());

            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            nameClaim.Should().NotBeNull();
            nameClaim!.Value.Should().Be(user.FullName);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            roleClaim.Should().NotBeNull();
            roleClaim!.Value.Should().Be(user.Role!.RoleName);
        }

        [Theory]
        //[MemberData(nameof(InvalidLoginData))]
        [InlineData("1234567890", "WrongPassword!")]
        [InlineData("9817232134", "YourStr0ngPassw0rd!")]
        [InlineData("0000000000", "WrongPassword!")]
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
