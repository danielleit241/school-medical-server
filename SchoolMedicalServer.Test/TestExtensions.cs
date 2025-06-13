using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Infrastructure;
using SchoolMedicalServer.Infrastructure.Repositories;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Test
{
    public class TestExtensions
    {
        public static SchoolMedicalManagementContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SchoolMedicalManagementContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new SchoolMedicalManagementContext(options);
        }

        public static IConfiguration CreateFakeConfig()
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
        public AuthService CreateAuthService(SchoolMedicalManagementContext context)
        {
            var baseRepo = new BaseRepository(context);
            var userRepo = new UserRepository(context);
            var config = CreateFakeConfig();
            return new AuthService(baseRepo, userRepo, config);
        }
    }
}
