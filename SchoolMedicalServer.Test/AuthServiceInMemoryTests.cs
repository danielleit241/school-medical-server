using System;
using System.Collections.Generic;
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
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "123456789012345678901234567890123456789012345678901234567890abcd",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["DefaultAccountCreate:Password"] = "Default@123"
        })
        .Build();
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

        [Theory]
        [MemberData(nameof(LoginTestData))]
        public async Task LoginAsync_DataDrivenTests(
            string seedPhone,
            string seedPassword,
            string loginPhone,
            string loginPassword,
            bool expectSuccess,
            bool expectToken)
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateContext(dbName);

            if (!string.IsNullOrEmpty(seedPhone) && !string.IsNullOrEmpty(seedPassword))
            {
                await SeedUserAsync(context, seedPhone, seedPassword);
            }

            var userRepo = new UserRepository(context);
            var baseRepo = new BaseRepository(context);
            var authService = new AuthService(baseRepo, userRepo, CreateFakeConfig());

            var request = new UserLoginRequest
            {
                PhoneNumber = loginPhone,
                Password = loginPassword
            };

            // Act
            var result = await authService.LoginAsync(request);

            // Assert
            if (expectSuccess)
            {
                Assert.NotNull(result);
                if (expectToken)
                {
                    Assert.False(string.IsNullOrEmpty(result.AccessToken));
                }
            }
            else
            {
                Assert.Null(result);
            }
        }

        public static IEnumerable<object[]> LoginTestData()
        {
            // seedPhone, seedPassword, loginPhone, loginPassword, expectSuccess, expectToken
            yield return new object[] { "0123456789", "TestPassword!1", "0123456789", "TestPassword!1", true, true }; // success
            yield return new object[] { "", "", "0000000000", "AnyPassword", false, false }; // user not exists
            yield return new object[] { "0123456789", "CorrectPassword", "0123456789", "WrongPassword", false, false }; // wrong password
            yield return new object[] { "0123456789", "Default@123", "0123456789", "Default@123", false, false }; // default password
        }
    }
}