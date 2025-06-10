using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Infrastructure;
using SchoolMedicalServer.Infrastructure.Repositories;
using SchoolMedicalServer.Infrastructure.Services;
using Xunit;

namespace SchoolMedicalServer.Tests.Services
{
    public class AuthServiceInMemoryTests
    {
        private SchoolMedicalManagementContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<SchoolMedicalManagementContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new SchoolMedicalManagementContext(options);
        }

        private IConfiguration CreateFakeConfig()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
         
            new KeyValuePair<string, string>("Jwt:Key", "123456789012345678901234567890123456789012345678901234567890abcd"),
            new KeyValuePair<string, string>("Jwt:Issuer", "TestIssuer"),
            new KeyValuePair<string, string>("Jwt:Audience", "TestAudience"),
            new KeyValuePair<string, string>("DefaultAccountCreate:Password", "Default@123")
                }).Build();
            return config;
        }

        private async Task SeedUserAsync(
            SchoolMedicalManagementContext context,
            string phone,
            string password,
            string roleName = "Admin",
            bool status = true)
        {
            var role = new Role { RoleId = 1, RoleName = roleName };
            var user = new User
            {
                UserId = Guid.NewGuid(),
                PhoneNumber = phone,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, password),
                RoleId = role.RoleId,
                Role = role,
                Status = status
            };
            await context.Roles.AddAsync(role);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task LoginAsync_Success_When_Correct_Account_And_Password()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateContext(dbName);
            await SeedUserAsync(context, "0123456789", "TestPassword!1");
            var userRepo = new UserRepository(context);
            var baseRepo = new BaseRepository(context);
            var authService = new AuthService(baseRepo, userRepo, CreateFakeConfig());

            var request = new UserLoginRequest
            {
                PhoneNumber = "0123456789",
                Password = "TestPassword!1"
            };

            // Act
            var result = await authService.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.AccessToken));
        }

        [Fact]
        public async Task LoginAsync_Fail_When_User_Not_Exists()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateContext(dbName);
            var userRepo = new UserRepository(context);
            var baseRepo = new BaseRepository(context);
            var authService = new AuthService(baseRepo, userRepo, CreateFakeConfig());

            var request = new UserLoginRequest
            {
                PhoneNumber = "0000000000",
                Password = "AnyPassword"
            };

            // Act
            var result = await authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_Fail_When_Wrong_Password()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateContext(dbName);
            await SeedUserAsync(context, "0123456789", "CorrectPassword");
            var userRepo = new UserRepository(context);
            var baseRepo = new BaseRepository(context);
            var authService = new AuthService(baseRepo, userRepo, CreateFakeConfig());

            var request = new UserLoginRequest
            {
                PhoneNumber = "0123456789",
                Password = "WrongPassword"
            };

            // Act
            var result = await authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_Fail_When_Using_DefaultPassword()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateContext(dbName);
            await SeedUserAsync(context, "0123456789", "Default@123");
            var userRepo = new UserRepository(context);
            var baseRepo = new BaseRepository(context);
            var authService = new AuthService(baseRepo, userRepo, CreateFakeConfig());

            var request = new UserLoginRequest
            {
                PhoneNumber = "0123456789",
                Password = "Default@123" // Mật khẩu mặc định, sẽ bị chặn
            };

            // Act
            var result = await authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }
    }
}