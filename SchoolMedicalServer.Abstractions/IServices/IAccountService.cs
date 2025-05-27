using SchoolMedicalServer.Abstractions.Dtos.Account;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAccountService
    {
        Task<List<AccountDto>> BatchCreateParentsAsync();
        Task<AccountDto?> RegisterStaffAsync(RegisterStaffRequest request);
    }
}
