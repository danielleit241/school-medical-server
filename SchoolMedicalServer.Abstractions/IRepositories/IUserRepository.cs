using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetByPhoneAndEmailAsync(string phoneNumber, string email);
        Task<User?> GetByIdAsync(Guid? userId);
        Task<User?> GetByOtpAsync(string otp);
        Task<User?> GetByPhoneAndOtpAsync(string phoneNumber, string otp);
        Task<User?> GetUserByRoleName(string roleName);
        Task<IEnumerable<User>> GetUsersByRoleName(string roleName);
        void Update(User user);
        Task<Dictionary<string, User>> GetUsersByPhoneNumbersAsync(List<string> phoneNumbers);
        Task AddUserAsync(User user);
        Task<bool> UserExistsByPhoneNumberAsync(string phoneNumber);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<int> CountByRoleIdAsync(int roleId);
        Task<List<User>> GetUsersByRoleIdPagedAsync(int roleId, string search, string sortBy, string sortOrder, int skip, int take);
    }
}
