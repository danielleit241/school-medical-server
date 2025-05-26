using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAccountServices
    {
        Task<User?> RegisterStaffAsync(RegisterRequestDto request);
    }
}
