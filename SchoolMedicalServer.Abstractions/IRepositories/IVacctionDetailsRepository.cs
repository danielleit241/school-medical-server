using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVacctionDetailsRepository
    {
        Task<List<VaccinationDetail>> GetAllAsync();
        Task<VaccinationDetail?> GetByIdAsync(Guid? id);
        Task<bool> IsExistsAsync(string code);
        Task<List<VaccinationDetail>> GetPagedAsync(int skip, int take);
        Task<int> CountAsync();
        Task AddAsync(VaccinationDetail vacctionDetails);
        void Update(VaccinationDetail vacctionDetails);
        void Delete(Guid id);
    }
}
