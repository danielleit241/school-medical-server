using SchoolMedicalServer.Abstractions.Dtos.Notification;
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
            if (result == null || result!.HealthQualified == false)
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

        public async Task<VaccinationResultResponse> GetVaccinationResult(Guid resultId)
        {
            var result = await resultRepository.GetByIdAsync(resultId);
            if (result == null) return null!;
            var res = new VaccinationResultResponse
            {
                ResultResponse = new VaccinationResultInformationResponse
                {
                    VaccinationResultId = result.VaccinationResultId,
                    HealthProfileId = result.HealthProfileId,
                    RecorderId = result.RecorderId,
                    VaccineId = result.Round?.Schedule?.Vaccine?.VaccineId ?? Guid.Empty,
                    Vaccinated = result.Vaccinated,
                    VaccinatedDate = result.VaccinatedDate,
                    VaccinatedTime = result.VaccinatedTime,
                    InjectionSite = result.InjectionSite,
                    Notes = result.Notes,
                    Status = result.Status,
                    ParentConfirmed = result.ParentConfirmed,
                    HealthQualified = result.HealthQualified,
                },
                VaccinationObservation = result.VaccinationObservation != null
                    ? new VaccinationObservationInformationResponse
                    {
                        ObservationStartTime = result.VaccinationObservation.ObservationStartTime,
                        ObservationEndTime = result.VaccinationObservation.ObservationEndTime,
                        ReactionStartTime = result.VaccinationObservation.ReactionStartTime,
                        ReactionType = result.VaccinationObservation.ReactionType,
                        SeverityLevel = result.VaccinationObservation.SeverityLevel,
                        ImmediateReaction = result.VaccinationObservation.ImmediateReaction,
                        Intervention = result.VaccinationObservation.Intervention,
                        ObservedBy = result.VaccinationObservation.ObservedBy,
                        Notes = result.VaccinationObservation.Notes
                    }
                    : null
            };
            return res;
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

        public async Task<bool?> GetHealthQualifiedVaccinationResult(Guid resultId)
        {
            var result = await resultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return false;
            }
            return result.HealthQualified;
        }

        public async Task<bool> UpdateHealthQualifiedVaccinationResult(Guid resultId, bool status)
        {
            var result = await resultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return false;
            }
            result.HealthQualified = status;
            if (result.HealthQualified == false)
            {
                result.InjectionSite = null;
                result.Vaccinated = false;
                result.VaccinatedDate = null;
                result.VaccinatedTime = null;
                result.Status = "Failed";
            }
            await resultRepository.UpdateAsync(result);
            return true;
        }

        public async Task<IEnumerable<VaccinationResultParentResponse>> GetVaccinationResultStudentAsync(Guid studentId)
        {
            var healthProfile = await healthProfileRepository.GetByStudentIdAsync(studentId);
            var results = await resultRepository.GetByHealthProfileId(healthProfile!.HealthProfileId);
            if (results == null || !results.Any())
            {
                return [];
            }
            results = [.. results.OrderByDescending(vr => vr!.VaccinatedDate).ThenByDescending(vr => vr!.VaccinatedTime)];

            var vaccineSummaries = results
                .Where(vr => vr?.Vaccinated == true && vr?.Round?.Schedule?.Vaccine != null)
                .GroupBy(vr => vr!.Round!.Schedule!.Vaccine!.VaccineName)
                .Select(g =>
                {
                    var orderedDetails = g
                        .OrderBy(vr => vr!.VaccinatedDate)
                        .ThenBy(vr => vr!.VaccinatedTime)
                        .Select((vr, idx) => new VaccineResultDetailResponse
                        {
                            VaccinationResultId = vr!.VaccinationResultId,
                            VaccinatedDate = vr.VaccinatedDate,
                            InjectionSite = vr.InjectionSite,
                            Manufacturer = vr.Round!.Schedule!.Vaccine!.Manufacturer,
                            BatchNumber = vr.Round!.Schedule!.Vaccine!.BatchNumber,
                            DoseNumber = idx + 1
                        })
                        .ToList();

                    return new VaccinationResultParentResponse
                    {
                        VaccineDoseSummary = new VaccineDoseSummary
                        {
                            VaccineName = g.Key,
                            TotalDoseByVaccineName = g.Count().ToString(),
                            VaccineResultDetails = orderedDetails
                        }
                    };
                })
                .ToList();

            return vaccineSummaries;
        }
    }
}
