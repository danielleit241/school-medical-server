using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;


namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class UserRepository(SchoolMedicalManagementContext _context) : IUserRepository
    {
        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber) =>
            await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Status == true);

        public async Task<User?> GetByPhoneAndEmailAsync(string phoneNumber, string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.EmailAddress == email);

        public async Task<User?> GetByIdAsync(Guid? userId) =>
            await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);

        public async Task<User?> GetByOtpAsync(string otp) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Otp == otp && u.OtpExpiryTime > DateTime.UtcNow);

        public async Task<User?> GetByPhoneAndOtpAsync(string phoneNumber, string otp) =>
            await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Otp == otp);

        public void Update(User user) => _context.Users.Update(user);

        public async Task<Dictionary<string, User>> GetUsersByPhoneNumbersAsync(List<string> phoneNumbers) =>
            await _context.Users
                .Where(u => phoneNumbers.Contains(u.PhoneNumber))
                .ToDictionaryAsync(u => u.PhoneNumber);

        public async Task<Role?> GetRoleByNameAsync(string roleName) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

        public async Task AddUserAsync(User user) =>
            await _context.Users.AddAsync(user);

        public async Task<bool> UserExistsByPhoneNumberAsync(string phoneNumber) =>
            await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);

        public async Task<User?> GetUserByRoleName(string roleName) =>
            await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Role != null && u.Role.RoleName == roleName && u.Status == true);
        public async Task<IEnumerable<User>> GetUsersByRoleName(string roleName) =>
            await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null && u.Role.RoleName == roleName && u.Status == true)
                .ToListAsync();
        public async Task<int> CountByRoleIdAsync(int roleId)
             => await _context.Users.CountAsync(u => u.RoleId == roleId);


        public async Task<List<User>> GetUsersByRoleIdPagedAsync(
            int roleId,
            string? search,
            string? sortBy,
            string? sortOrder,
            int skip,
            int take)
        {
            IQueryable<User> query = _context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(u => u.FullName!.ToLower().Contains(lowerSearch));
            }

            string defaultSort = "FullName ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Status == true)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
