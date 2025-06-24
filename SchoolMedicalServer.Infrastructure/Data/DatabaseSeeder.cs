using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly SchoolMedicalManagementContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(
            SchoolMedicalManagementContext context,
            IConfiguration configuration,
            ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                // Ensure database is created and migrated
                if (await _context.Database.CanConnectAsync())
                {
                    await _context.Database.MigrateAsync();
                    _logger.LogInformation("Database migration completed successfully");
                }

                // Seed in order
                await SeedRolesAsync();
                await SeedDefaultAdminAsync();

                _logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            if (await _context.Roles.AnyAsync())
            {
                _logger.LogInformation("Roles already exist, skipping role seeding");
                return;
            }

            var roles = new[]
            {
                new Role { RoleId = 1, RoleName = "admin"},
                new Role { RoleId = 2, RoleName = "nurse"},
                new Role { RoleId = 3, RoleName = "manager"},
                new Role { RoleId = 4, RoleName = "parent"}
            };

            _context.Roles.AddRange(roles);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Roles seeded successfully");
        }

        private async Task SeedDefaultAdminAsync()
        {
            if (await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Users already exist, skipping admin seeding");
                return;
            }

            var adminSection = _configuration.GetSection("DefaultAdmin");
            var phoneNumber = adminSection["PhoneNumber"];
            var password = adminSection["Password"];
            var fullName = "System Administrator";

            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Default admin configuration is missing");
            }

            var passwordHasher = new PasswordHasher<User>();
            var admin = new User
            {
                UserId = Guid.NewGuid(),
                PhoneNumber = phoneNumber,
                FullName = fullName,
                PasswordHash = passwordHasher.HashPassword(null!, password),
                RoleId = 1, // Admin role
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Default admin user created successfully");
        }
    }
}