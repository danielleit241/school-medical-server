using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SchoolMedicalServer.Abstractions.Dtos.Dashboard;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AdminDashboardService(
        IUserRepository userRepository,
        IConfiguration configuration) : IAdminDashboardService
    {
        public async Task<IEnumerable<DashboardResponse>> GetColumnDataUsersAsync(DashboardRequest request)
        {
            List<DashboardResponse> responses = [];
            var passwordHasher = new PasswordHasher<User>();
            var defaultPassword = configuration["DefaultAccountCreate:Password"];
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));


            var users = await userRepository.GetAllUser();
            users = [.. users
                    .Where(u =>  u.Status == true &&
                            fromDate <= u.CreatedAt!.Value &&
                            toDate >= u.CreatedAt.Value)];

            var usersChangePassword = users.Where(u => passwordHasher.VerifyHashedPassword(u, u.PasswordHash, defaultPassword!) != PasswordVerificationResult.Success).ToList();
            var usersNotChangePassword = users.Where(u => passwordHasher.VerifyHashedPassword(u, u.PasswordHash, defaultPassword!) == PasswordVerificationResult.Success).ToList();

            var totalUsers = users.ToList().Count;

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Total Users in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = totalUsers
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Password Changed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = usersChangePassword.Count,
                    Details = usersChangePassword.Select(u => new ItemDetails
                    {
                        Id = u.UserId,
                        Name = u.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Default Password in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = usersNotChangePassword.Count,
                    Details = usersNotChangePassword.Select(u => new ItemDetails
                    {
                        Id = u.UserId,
                        Name = u.FullName
                    }).ToList()
                }
            });
            return responses;
        }

        public async Task<IEnumerable<DashboardUserRecentActionResponse>> GetRecentActionsAsync(DashboardRequest request)
        {
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));

            var responses = new List<DashboardUserRecentActionResponse>();
            var user = await userRepository.GetAllUser();
            var userJustCreated = user.Where(u => u.CreatedAt >= fromDate && u.CreatedAt <= toDate).ToList();
            var userJustUpdated = user.Where(u => u.UpdatedAt >= fromDate && u.UpdatedAt <= toDate).ToList();
            var userJustResetPassword = user.Where(u => u.OtpExpiryTime >= fromDate && u.OtpExpiryTime <= toDate).ToList();

            responses.Add(new DashboardUserRecentActionResponse
            {
                UserRecentAction = new UserRecentAction
                {
                    Name = $"Create in {fromDate} to {toDate}",
                    Count = userJustCreated.Count
                }
            });

            responses.Add(new DashboardUserRecentActionResponse
            {
                UserRecentAction = new UserRecentAction
                {
                    Name = $"Update in {fromDate} to {toDate}",
                    Count = userJustUpdated.Count
                }
            });

            responses.Add(new DashboardUserRecentActionResponse
            {
                UserRecentAction = new UserRecentAction
                {
                    Name = $"Reset password in {fromDate} to {toDate}",
                    Count = userJustResetPassword.Count
                }
            });

            return responses;
        }
    }
}
