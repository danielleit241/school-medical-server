using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserService(
        IBaseRepository baseRepository,
        IUserRepository userRepository,
        IHealthCheckRoundRepository healthCheckRoundRepository,
        IVaccinationRoundRepository vaccinationRoundRepository) : IUserService
    {
        public async Task<PaginationResponse<UserInformation?>> GetUsersByRoleNamePaginationAsync(PaginationRequest? paginationRequest, string roleName)
        {
            var role = await userRepository.GetRoleByNameAsync(roleName);
            if (role == null) return null!;

            var totalCount = await userRepository.CountByRoleIdAsync(role.RoleId);
            int skip = (paginationRequest!.PageIndex - 1) * paginationRequest.PageSize;
            var users = await userRepository.GetUsersByRoleIdPagedAsync(
                role.RoleId,
                paginationRequest.Search!,
                paginationRequest.SortBy!,
                paginationRequest.SortOrder!,
                skip, paginationRequest.PageSize);

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
            )!;
        }

        public async Task<UserInformation?> GetUserAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
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
            var user = await userRepository.GetByIdAsync(userid);
            if (user == null)
            {
                return false;
            }
            user.Status = status;
            user.UpdatedAt = DateTime.UtcNow;
            userRepository.Update(user);

            await baseRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(Guid userid, UserInformation request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return false;
            }
            if (request == null)
            {
                return false;
            }

            var newRole = await userRepository.GetRoleByNameAsync(request!.RoleName!);

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
            user.UpdatedAt = DateTime.UtcNow;

            userRepository.Update(user);

            await baseRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserInformation>> GetFreeNursesAsync()
        {
            DateTime todayStart = DateTime.Today;
            DateTime todayEnd = todayStart.AddDays(1).AddTicks(-1);
            var vaccinationRounds = await vaccinationRoundRepository.GetVaccinationRoundsAsync();
            var healthCheckRounds = await healthCheckRoundRepository.GetHealthCheckRoundsAsync();
            var nurses = await userRepository.GetUsersByRoleName("nurse");

            var freeNurses = new List<UserInformation>();
            foreach (var nurse in nurses)
            {
                var hasVaccinationToday = vaccinationRounds.Any(vr =>
                            vr.NurseId == nurse.UserId &&
                            vr.StartTime <= todayEnd &&
                            vr.EndTime >= todayStart
                        );

                var hasHealthCheckToday = healthCheckRounds.Any(hcr =>
                            hcr.NurseId == nurse.UserId &&
                            hcr.StartTime <= todayEnd &&
                            hcr.EndTime >= todayStart
                        );
                if (!hasVaccinationToday && !hasHealthCheckToday)
                {
                    freeNurses.Add(new UserInformation
                    {
                        UserId = nurse.UserId,
                        FullName = nurse.FullName,
                        PhoneNumber = nurse.PhoneNumber,
                        EmailAddress = nurse.EmailAddress,
                        AvatarUrl = nurse.AvatarUrl ?? "",
                        DayOfBirth = nurse.DayOfBirth,
                        RoleName = nurse.Role?.RoleName ?? "",
                        Status = nurse.Status ?? false,
                        Address = nurse.Address ?? ""
                    });
                }
            }
            return freeNurses;
        }
    }
}
