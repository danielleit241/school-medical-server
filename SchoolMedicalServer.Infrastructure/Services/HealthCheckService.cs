using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckService(IHealthCheckRepository healthCheckRepository) : IHealthCheckService
    {
        public async Task<bool> CreateScheduleAsync(HealthCheckScheduleRequest request)
        {
            if (request == null)
            {
                return false!;
            }
            var healthCheckSchedule = new HealthCheckSchedule
            {
                ScheduleId = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                HealthCheckType = request.HealthCheckType,
                CreatedBy = request.CreatedBy,
                ParentNotificationStartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ParentNotificationEndDate = request.StartDate!.Value.AddDays(-1),
                Rounds = [.. request.HealthCheckRounds.Select(round => new HealthCheckRound
                {
                    RoundId = Guid.NewGuid(),
                    RoundName = round.RoundName,
                    TargetGrade = round.TargetGrade,
                    Description = round.Description,
                    StartTime = round.StartTime,
                    EndTime = round.EndTime,
                    NurseId = round.NurseId,
                })]
            };
            await healthCheckRepository.CreateHealthCheckSchedule(healthCheckSchedule);
            return true;
        }

        public Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId)
        {
            throw new NotImplementedException();
        }

        public Task<HealthCheckScheduleDetailsResponse> GetHealthCheckSchedule(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateScheduleAsync(Guid scheduleId, HealthCheckScheduleUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        Task<PaginationResponse<HealthCheckScheduleResponse>> IHealthCheckService.GetPaginationHealthCheckSchedule(PaginationRequest? pagination)
        {
            throw new NotImplementedException();
        }
    }
}
