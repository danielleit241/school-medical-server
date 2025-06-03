using Azure;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserService(SchoolMedicalManagementContext context) : IUserService
    {
        public async Task<PaginationResponse<UserInformation>> GetUsersByRoleNamePaginationAsync(PaginationRequest paginationRequest, string roleName)
        {
            if (paginationRequest == null)
            {
                paginationRequest = new PaginationRequest();
            }
            if (paginationRequest.PageIndex <= 0)
            {
                paginationRequest.PageIndex = 1;
            }
            if (paginationRequest.PageSize <= 0)
            {
                paginationRequest.PageSize = 10;
            }
            var role = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null) return null;

            var totalCount = await context.Users.CountAsync();
            var users = await context.Users.Include(u => u.Role)
                .Where(u => u.RoleId == role.RoleId)
                .OrderBy(u => u.FullName)
                .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();
            if (users == null) return null!;

            var userDtos = new List<UserInformation>();

            foreach (var user in users)
            {
                userDtos.Add(new UserInformation
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    EmailAddress = user.EmailAddress,
                    AvatarUrl = user.AvatarUrl ?? "",
                    DayOfBirth = user.DayOfBirth,
                    RoleName = user.Role?.RoleName ?? "",
                    Status = user.Status ?? false,
                    Address = user.Address ?? ""
                });
            }
            return new PaginationResponse<UserInformation>(
                    paginationRequest.PageIndex,
                    paginationRequest.PageSize,
                    totalCount,
                    userDtos
            );
        }

        public async Task<UserInformation?> GetUserAsync(Guid userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return null;
            var response = new UserInformation
            {
                UserId = user.UserId,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                EmailAddress = user.EmailAddress,
                AvatarUrl = user.AvatarUrl ?? "",
                DayOfBirth = user.DayOfBirth,
                RoleName = user.Role?.RoleName ?? "",
                Status = user.Status ?? false,
                Address = user.Address ?? ""
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

        public async Task<bool> UpdateUserAsync(Guid userid, UserInformation request)
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
            user.PhoneNumber = request.PhoneNumber!;
            user.EmailAddress = request.EmailAddress;
            user.DayOfBirth = request.DayOfBirth;
            user.AvatarUrl = request.AvatarUrl;
            user.RoleId = newRole.RoleId;
            user.Address = request.Address;

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
