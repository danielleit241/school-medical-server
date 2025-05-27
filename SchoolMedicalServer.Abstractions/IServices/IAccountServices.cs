using SchoolMedicalServer.Abstractions.Dtos.Account;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAccountServices
    {
        Task<List<AccountDto>> BatchCreateParentsAsync();
        Task<AccountDto?> RegisterStaffAsync(RegisterStaffRequest request);
    }
}
