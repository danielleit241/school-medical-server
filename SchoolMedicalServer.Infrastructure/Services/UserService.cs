using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Profile;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly SchoolMedicalManagementContext _context;

        public UserService(SchoolMedicalManagementContext context)
        {
            _context = context;
        }

        public async Task<List<UserDTO?>> GetAllAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(); // Fixed: Use FirstOrDefaultAsync for async operations
            if (user == null) return null;

            return new List<UserDTO?> // Fixed: Return a list of UserDTO
            {
                new UserDTO
                {
                    UserId = user.UserId,
                    RoleId = user.RoleId,
                    FullName = user.FullName,
                    EmailAddress = user.EmailAddress,
                    DayOfBirth = user.DateOfBirth,
                    AvatarUrl = user.AvatarURL
                }
            };
        }

        //public Task<UserDTO?> UpdateUserAsync(Guid userId, UserDTO dto)
        //{
        //    var user = _context.Users.FindAsync(userId);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    var userToUpdate = user.Result;
        //}
    }
}
