using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationDetailsRepository
    {
        Task<List<VaccinationDetail>> GetAllAsync();
        Task<VaccinationDetail?> GetByIdAsync(Guid? id);
        Task<bool> IsExistsAsync(string code);
        Task<List<VaccinationDetail>> GetPagedAsync(string search, string sortBy, string sortOrder, int skip, int take);
        Task<int> CountAsync();
        Task AddAsync(VaccinationDetail vacctionDetails);
        void Update(VaccinationDetail vacctionDetails);
        void Delete(VaccinationDetail vaccinationDetail);

    }
}
