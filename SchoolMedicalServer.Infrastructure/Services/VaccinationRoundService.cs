using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationRoundService(IVaccinationRoundRepository vaccinationRound) : IVaccinationRoundService
    {
        public Task<bool> GetStudentsByVacciantionRoundIdAsync(PaginationRequest? pagination, Guid roundId)
        {
            throw new NotImplementedException();
        }
    }
}
