using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Rounds;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationRoundService(
        IVaccinationRoundRepository vaccinationRound,
        IVaccinationResultRepository vaccinationResultRepository) : IVaccinationRoundService
    {
        public Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdAsync(PaginationRequest? pagination, Guid roundId)
        {
            throw new NotImplementedException();
        }

        public Task<VaccinationRoundResponse> GetVaccinationRoundByIdAsync(Guid roundId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<VaccinationRoundResponse>> GetVaccinationRoundsAsync(PaginationRequest? pagination)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<VaccinationRoundResponse>> GetVaccinationRoundsByUserIdAsync(Guid userId, PaginationRequest? pagination)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateVaccinationRoundAsync(Guid roundId, VaccinationRoundRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
