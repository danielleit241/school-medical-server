using Azure;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserService(SchoolMedicalManagementContext context) : IUserService
    {
        public async Task<PaginationResponse<UserDto>> GetAllAsync(PaginationRequest paginationRequest)
        {
            var totalCount = await context.Users.CountAsync();
            var users = await context.Users.Include(u => u.Role)
                .OrderBy(u => u.FullName)
                .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();
            if (users == null) return null!;

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    EmailAddress = user.EmailAddress,
                    AvatarUrl = user.AvatarUrl ?? "",
                    DayOfBirth = user.DayOfBirth,
                    RoleName = user.Role?.RoleName ?? "",
                    Status = user.Status ?? false
                });
            }
            return new PaginationResponse<UserDto>(
                    paginationRequest.PageIndex,
                    paginationRequest.PageSize,
                    totalCount,
                    userDtos
            );
        }

        public async Task<UserDto?> GetUserAsync(Guid userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return null;
            var response = new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                EmailAddress = user.EmailAddress,
                AvatarUrl = user.AvatarUrl ?? "",
                DayOfBirth = user.DayOfBirth,
                RoleName = user.Role?.RoleName ?? "",
                Status = user.Status ?? false
            };
            return response;
        }

        public async Task<bool> UpdateStatusUserAsync(Guid userid, bool status)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == userid);
            if (user == null)
            {
                return false;
            }
            user.Status = status;
            context.Users.Update(user);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(Guid userid, UserDto request)
        {
            var user = await context.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.UserId == userid);
            if (user == null)
            {
                return false;
            }
            if (request == null)
            {
                return false;
            }

            var newRole = await context.Roles.FirstOrDefaultAsync(x => x.RoleName == request.RoleName);

            if (newRole == null)
            {
                return false;
            }

            user.FullName = request.FullName;
            user.EmailAddress = request.EmailAddress;
            user.DayOfBirth = request.DayOfBirth;
            user.AvatarUrl = request.AvatarUrl;
            user.RoleId = newRole.RoleId;

            context.Users.Update(user);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
