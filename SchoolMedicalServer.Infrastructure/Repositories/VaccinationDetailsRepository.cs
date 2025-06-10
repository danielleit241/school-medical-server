using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationDetailsRepository(SchoolMedicalManagementContext _context) : IVacctionDetailsRepository
    {
        public async Task AddAsync(VaccinationDetail vacctionDetails)
        {
            await _context.VaccinationDetails.AddAsync(vacctionDetails);
        }

        public async Task<int> CountAsync()
        {
            return await _context.VaccinationDetails.CountAsync();
        }

        public void Delete(Guid id)
        {
            _context.VaccinationDetails.Remove(new VaccinationDetail { VaccineId = id });
        }

        public async Task<List<VaccinationDetail>> GetAllAsync()
        {
            return await _context.VaccinationDetails.ToListAsync();
        }

        public async Task<VaccinationDetail?> GetByIdAsync(Guid id)
        {
            return await _context.VaccinationDetails
                .FirstOrDefaultAsync(v => v.VaccineId == id);
        }

        public async Task<List<VaccinationDetail>> GetPagedAsync(int skip, int take)
        {
            return await _context.VaccinationDetails
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<bool> IsExistsAsync(string code)
        {
            return await _context.VaccinationDetails
                .AnyAsync(v => v.VaccineCode == code);
        }

        public void Update(VaccinationDetail vacctionDetails)
        {
            _context.VaccinationDetails.Update(vacctionDetails);
        }
    }
}
