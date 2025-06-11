
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationRoundService
    {
        Task<bool> GetStudentsByVacciantionRoundIdAsync(PaginationRequest? pagination, Guid roundId);
    }
}
