using SchoolMedicalServer.Abstractions.Dtos.Account;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAccountService
    {
        Task<List<AccountResponse>> BatchCreateParentsAsync();
        Task<AccountResponse?> RegisterStaffAsync(RegisterStaffRequest request);
    }
}
