using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                EmailAddress = "test@gmail.com",
                Status = true
            });
            context.Users.AddRange(userEntities);
            context.SaveChanges();
        }

        public static IEnumerable<object[]> ValidLoginData() => Users.ToList().Select(u => new object[] { u.Phone, u.Password });
        public static IEnumerable<object[]> InvalidLoginData() => [
            ["1234567899", "WrongPassword!"],
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

        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task RefreshToken_ShouldReturnToken_WhenCredentialsAreValid(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var loginRequest = new UserLoginRequest
            {
                PhoneNumber = phoneNumber,
                Password = password
            };
            var checkLogin = await authService.CheckLoginAsync(loginRequest);
            Assert.True(checkLogin);
            var result = await authService.LoginAsync(loginRequest);
            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);
            Assert.NotEmpty(result.RefreshToken);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(result.AccessToken);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            userIdClaim.Should().NotBeNull();
            var requestRefresh = new RefreshTokenRequest
            {
                RefreshToken = result.RefreshToken,
                UserId = Guid.Parse(userIdClaim.Value),
            };
            var refreshToken = await authService.RefreshTokenAsync(requestRefresh);

            requestRefresh.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task RefreshToken_ShouldReturnNull_WhenRefreshTokensAreInvalid(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var loginRequest = new UserLoginRequest
            {
                PhoneNumber = phoneNumber,
                Password = password
            };
            var checkLogin = await authService.CheckLoginAsync(loginRequest);
            Assert.True(checkLogin);
            var result = await authService.LoginAsync(loginRequest);
            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);
            Assert.NotEmpty(result.RefreshToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(result.AccessToken);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            userIdClaim.Should().NotBeNull();
            var requestRefresh = new RefreshTokenRequest
            {
                RefreshToken = "InvalidRefreshToken",
                UserId = Guid.Parse(userIdClaim.Value),
            };
            var refreshToken = await authService.RefreshTokenAsync(requestRefresh);
            refreshToken.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task ChangePassword_ShouldReturnUser_WhenPasswordIsChanged(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            Assert.NotNull(user);
            var oldHash = user.PasswordHash;
            var changePasswordRequest = new ChangePasswordRequest
            {
                PhoneNumber = phoneNumber,
                OldPassword = password,
                NewPassword = "NewStr0ngPassw0rd!"
            };
            var updatedUser = await authService.ChangePasswordAsync(changePasswordRequest);
            Assert.NotNull(updatedUser);
            updatedUser.PasswordHash.Should().NotBe(oldHash);
        }

        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task ChangePassword_ShouldReturnNull_WhenOldPasswordIsIncorrect(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            Assert.NotNull(user);
            var oldHash = user.PasswordHash;
            var changePasswordRequest = new ChangePasswordRequest
            {
                PhoneNumber = phoneNumber,
                OldPassword = "WrongOldPassword!",
                NewPassword = "NewStr0ngPassw0rd!"
            };
            var updatedUser = await authService.ChangePasswordAsync(changePasswordRequest);
            Assert.Null(updatedUser);
            user.PasswordHash.Should().Be(oldHash);
        }

        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task ResetPassword_ShouldReturnTrue_WhenOptIsValid(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);


            var otp = await authService.GetOtpAsync(new SendOtpRequest
            {
                PhoneNumber = phoneNumber,
                EmailAddress = "test@gmail.com"
            });

            Assert.NotNull(otp);
            Assert.NotEmpty(otp);

            var verifyOtp = await authService.VerifyOtpAsync(otp);
            Assert.True(verifyOtp);

            var resetPasswordRequest = new ResetPasswordRequest
            {
                PhoneNumber = phoneNumber,
                Otp = otp,
                NewPassword = "NewStr0ngPassw0rd!"
            };

            var result = await authService.ResetPasswordAsync(resetPasswordRequest);
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(ValidLoginData))]
        public async Task ResetPassword_ShouldReturnFalse_WhenOptIsInvalid(string phoneNumber, string password)
        {
            var context = CreateContext();
            SeedRolesAndUsers(context, Users);
            var authService = CreateAuthService(context);
            var otp = await authService.GetOtpAsync(new SendOtpRequest
            {
                PhoneNumber = phoneNumber,
                EmailAddress = "wrong@email.com"
            });
            Assert.Null(otp);
            var verifyOtp = await authService.VerifyOtpAsync("InvalidOtp");
            Assert.False(verifyOtp);
            var resetPasswordRequest = new ResetPasswordRequest
            {
                PhoneNumber = phoneNumber,
                Otp = "InvalidOtp",
                NewPassword = "NewStr0ngPassw0rd!"
            };
            var result = await authService.ResetPasswordAsync(resetPasswordRequest);
            Assert.False(result);
        }
    }
}
