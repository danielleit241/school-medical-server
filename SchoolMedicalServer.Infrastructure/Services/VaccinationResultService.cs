using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationResultService(
        IVaccinationResultRepository resultRepository,
        IVaccinationRoundRepository roundRepository,
        IVaccinationScheduleRepository scheduleRepository,
        IVaccinationObservationRepository observationRepository,
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
            if (result.ParentConfirmed.HasValue)
            {
                return result.ParentConfirmed;
            }
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (today < schedule.ParentNotificationStartDate || today > schedule.ParentNotificationEndDate)
            {
                return false;
            }
            result.ParentConfirmed = request.Status;
            if (result.ParentConfirmed == false)
            {
                result.Notes = "Parent declined vaccination.";
                result.Status = "Declined";
            }
            resultRepository.Update(result);
            await baseRepository.SaveChangesAsync();
            return result.ParentConfirmed;
        }

        public Task<NotificationRequest> CreateVaccinationObservation(VaccinationObservationRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateVaccinationResult(VaccinationResultRequest request)
        {
            var result = await resultRepository.GetByIdAsync(request.VaccinationResultId);
            if (result == null)
            {
                return false;
            }
            result.Vaccinated = request.Vaccinated;
            result.VaccinatedDate = request.VaccinatedDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            result.InjectionSite = request.InjectionSite;
            result.Notes = request.Notes;
            result.Status = request.Status ?? "Failed";

            resultRepository.Update(result);
            await baseRepository.SaveChangesAsync();
            return true;
        }

        public Task<VaccinationResultResponse> GetVaccinationResult(Guid resultId)
        {
            throw new NotImplementedException();
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
