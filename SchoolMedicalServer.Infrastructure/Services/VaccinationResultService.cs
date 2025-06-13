using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationResultService(
        IVaccinationResultRepository resultRepository,
        IVaccinationRoundRepository roundRepository,
        IVaccinationScheduleRepository scheduleRepository,
        IBaseRepository baseRepository) : IVaccinationResultService
    {
        public async Task<bool?> ConfirmOrDeclineVaccination(Guid resultId, ParentVaccinationConfirmationRequest request)
        {
            var result = await resultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return false;
            }
            var round = await roundRepository.GetVaccinationRoundByIdAsync(result.RoundId);
            if (round == null)
            {
                return false;
            }
            var schedule = await scheduleRepository.GetVaccinationScheduleByIdAsync(round.ScheduleId);
            if (schedule == null)
            {
                return false;
            }
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (today < schedule.ParentNotificationStartDate || today > schedule.ParentNotificationEndDate)
            {
                return false;
            }
            if (result.ParentConfirmed == true)
            {
                return false;
            }
            result.ParentConfirmed = request.Status;
            resultRepository.Update(result);
            await baseRepository.SaveChangesAsync();
            return result.ParentConfirmed;
        }

        public async Task<bool?> IsVaccinationConfirmed(Guid resultId)
        {
            var result = await resultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return false;
            }
            return result.ParentConfirmed;
        }
    }
}
