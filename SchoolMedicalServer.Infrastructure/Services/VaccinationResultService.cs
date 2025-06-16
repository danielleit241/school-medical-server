using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationResultService(
        IVaccinationResultRepository resultRepository,
        IVaccinationRoundRepository roundRepository,
        IVaccinationScheduleRepository scheduleRepository,
        IVaccinationObservationRepository observationRepository,
        IHealthProfileRepository healthProfileRepository,
        IStudentRepository studentRepository) : IVaccinationResultService
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
            await resultRepository.UpdateAsync(result);
            return result.ParentConfirmed;
        }

        public async Task<NotificationRequest> CreateVaccinationObservation(VaccinationObservationRequest request)
        {
            var result = await resultRepository.GetByIdAsync(request.VaccinationResultId);
            if (result == null)
            {
                return null!;
            }
            if (result.Vaccinated == false)
            {
                return null!;
            }
            if (await observationRepository.IsExistResultIdAsync(request.VaccinationResultId))
            {
                return null!;
            }
            var observation = new VaccinationObservation
            {
                VaccinationObservationId = Guid.NewGuid(),
                VaccinationResultId = request.VaccinationResultId,
                ObservationStartTime = request.ObservationStartTime,
                ObservationEndTime = request.ObservationEndTime,
                ReactionStartTime = request.ReactionStartTime,
                ImmediateReaction = request.ImmediateReaction,
                ObservedBy = request.ObservedBy,
                Intervention = request.Intervention,
                ReactionType = request.ReactionType,
                SeverityLevel = request.SeverityLevel,
                Notes = request.Notes,
            };

            var healthProfile = await healthProfileRepository.GetHealthProfileById(result.HealthProfileId);
            var parentId = await studentRepository.GetParentUserIdAsync(healthProfile!.StudentId);
            await observationRepository.CreateVaccinationObservation(observation);

            return new NotificationRequest
            {
                NotificationTypeId = observation.VaccinationObservationId,
                SenderId = result.Round!.NurseId,
                ReceiverId = parentId,
            };
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
            result.VaccinatedTime = request.VaccinatedTime ?? DateTime.UtcNow;
            result.InjectionSite = request.InjectionSite;
            result.Notes = request.Notes;
            result.Status = request.Status ?? "Failed";

            await resultRepository.UpdateAsync(result);
            return true;
        }

        public async Task<VaccinationResultInformationResponse> GetVaccinationResult(Guid resultId)
        {
            var result = await resultRepository.GetByIdAsync(resultId);
            if (result == null) return null!;
            return MapToVaccinationResultResponse(result);
        }


        private static VaccinationResultInformationResponse MapToVaccinationResultResponse(VaccinationResult result) => new VaccinationResultInformationResponse
        {
            VaccinationResultId = result.VaccinationResultId,
            RoundId = result.RoundId,
            HealthProfileId = result.HealthProfileId,
            ParentConfirmed = result.ParentConfirmed,
            Vaccinated = result.Vaccinated,
            VaccinatedDate = result.VaccinatedDate,
            InjectionSite = result.InjectionSite,
            RecorderId = result.RecorderId,
            Status = result.Status,
            Notes = result.Notes,
            Observation = MapToObservationResponse(result.VaccinationObservation)
        };

        private static VaccinationObservationInformationResponse? MapToObservationResponse(VaccinationObservation? obs) =>
            obs == null ? null : new VaccinationObservationInformationResponse
            {
                ObservationStartTime = obs.ObservationStartTime,
                ObservationEndTime = obs.ObservationEndTime,
                ReactionStartTime = obs.ReactionStartTime,
                ReactionType = obs.ReactionType,
                SeverityLevel = obs.SeverityLevel,
                ImmediateReaction = obs.ImmediateReaction,
                Intervention = obs.Intervention,
                ObservedBy = obs.ObservedBy,
                Notes = obs.Notes
            };


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
