using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.User;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserService
    {
        Task<PaginationResponse<UserDto>> GetUsersByRoleNamePaginationAsync(PaginationRequest paginationRequest, string roleName);
        Task<UserDto?> GetUserAsync(Guid userId);
        Task<bool> UpdateStatusUserAsync(Guid userid, bool status);
        Task<bool> UpdateUserAsync(Guid userid, UserDto request);
    }
}
