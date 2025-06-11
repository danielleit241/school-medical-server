using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using System.Linq.Dynamic.Core;

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

        public async Task<VaccinationDetail?> GetByIdAsync(Guid? id)
        {
            return await _context.VaccinationDetails
                .FirstOrDefaultAsync(v => v.VaccineId == id);
        }

        public async Task<List<VaccinationDetail>> GetPagedAsync(
                string? search,
                string? sortBy,
                string? sortOrder,
                int skip,
                int take)
        {
            IQueryable<VaccinationDetail> query = _context.VaccinationDetails.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(s => s.VaccineCode!.ToLower().Contains(lowerSearch));
            }

            string defaultSort = "VaccineCode ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
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

        public void Delete(VaccinationDetail vaccinationDetail)
        {
            _context.VaccinationDetails.Remove(vaccinationDetail);
        }
    }
}
