using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Profile;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly SchoolMedicalManagementContext _context;
        private readonly IConfiguration _configuration;

        public UserProfileService(SchoolMedicalManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }   

  public async Task<UserProfileDTO?> GetUserProfileByIdAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserProfileDTO
            {
                FullName = user.FullName,
                Email = user.EmailAddress,
                DateOfBirth = user.DateOfBirth,
                AvatarURL = user.AvatarURL
            };
        }

        public async Task<UserProfileDTO?> UpdateUserProfileAsync(Guid userId, UserProfileDTO dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.FullName = dto.FullName;
            user.EmailAddress = dto.Email;
            user.DateOfBirth = dto.DateOfBirth;
            user.AvatarURL = dto.AvatarURL;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserProfileDTO
            {
                FullName = user.FullName,
                Email = user.EmailAddress,
                DateOfBirth = user.DateOfBirth,
                AvatarURL = user.AvatarURL
            };
        }
    }
}

