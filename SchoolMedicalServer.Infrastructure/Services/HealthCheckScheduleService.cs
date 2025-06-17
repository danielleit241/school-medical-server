using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckScheduleService(
        IHealthCheckScheduleRepository healthCheckRepository,
        IHealthCheckResultRepository resultRepository,
        IStudentRepository studentRepository,
        IHealthProfileRepository profileRepository,
        IHealthCheckRoundRepository roundRepository,
        IUserRepository userRepository) : IHealthCheckScheduleService
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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

        public async Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId)
        {
            var schedule = await healthCheckRepository.GetHealthCheckScheduleByIdAsync(scheduleId);
            if (schedule == null)
                return null!;
            var toParents = new List<NotificationRequest>();
            var toNurses = new List<NotificationRequest>();
            foreach (var round in schedule.Rounds)
            {
                if (await resultRepository.IsExistStudentByRoundId(round.RoundId))
                    continue;
                var students = await studentRepository.GetStudentsByGradeAsync(round.TargetGrade);
                foreach (var student in students)
                {
                    var healthProfile = await profileRepository.GetByStudentIdAsync(student.StudentId);
                    var result = new HealthCheckResult
                    {
                        ResultId = Guid.NewGuid(),
                        HealthProfileId = healthProfile!.HealthProfileId!,
                        RoundId = round.RoundId,
                        ParentConfirmed = null,
                        DatePerformed = null,
                        Height = null,
                        Weight = null,
                        VisionLeft = null,
                        VisionRight = null,
                        Hearing = null,
                        Nose = null,
                        BloodPressure = null,
                        Status = false,
                        Notes = null,
                        RecordedId = schedule.CreatedBy
                    };
                    await resultRepository.Create(result);
                    toParents.Add(new NotificationRequest
                    {
                        NotificationTypeId = result.ResultId,
                        SenderId = schedule.CreatedBy,
                        ReceiverId = student.UserId,
                    });
                }
                toNurses.Add(new NotificationRequest
                {
                    NotificationTypeId = scheduleId,
                    SenderId = schedule.CreatedBy,
                    ReceiverId = round.NurseId,
                });
            }
            return new NotificationScheduleResponse(toParents, toNurses); ;
        }

        public async Task<IEnumerable<HealthCheckScheduleDetailsResponse>> GetHealthCheckSchedule(Guid id)
        {
            var schedule = await healthCheckRepository.GetHealthCheckScheduleByIdAsync(id);
            if (schedule == null)
            {
                return null!;
            }
            var heathCheckRounds = await roundRepository.GetHealthCheckRoundsByScheduleIdAsync(id);
            if (heathCheckRounds == null)
            {
                return null!;
            }
            var res = new List<HealthCheckScheduleDetailsResponse>();
            foreach (var round in heathCheckRounds)
            {
                var detailsResponse = await MapToDetailsResponse(round);
                res.Add(detailsResponse);
            }
            return res;
        }

        private async Task<HealthCheckScheduleDetailsResponse> MapToDetailsResponse(HealthCheckRound round)
        {
            var nurseInfor = await userRepository.GetByIdAsync(round.NurseId);
            return new HealthCheckScheduleDetailsResponse
            {
                HealthCheckRoundInformation = new HealthCheckRoundInformationResponse
                {
                    RoundId = round.RoundId,
                    RoundName = round.RoundName ?? "",
                    TargetGrade = round.TargetGrade ?? "",
                    Description = round.Description ?? "",
                    StartTime = round.StartTime,
                    EndTime = round.EndTime,
                    Status = round.Status
                },
                Nurse = new HealthCheckRoundNurseInformationResponse
                {
                    NurseId = round.NurseId,
                    NurseName = nurseInfor!.FullName ?? "",
                    PhoneNumber = nurseInfor.PhoneNumber ?? "",
                    AvatarUrl = nurseInfor.AvatarUrl ?? string.Empty
                }
            };
        }

        public async Task<bool> UpdateScheduleAsync(Guid scheduleId, HealthCheckScheduleUpdateRequest request)
        {
            var schedule = await healthCheckRepository.GetHealthCheckScheduleByIdAsync(scheduleId);
            if (schedule == null)
            {
                return false;
            }
            schedule.Title = request.Title;
            schedule.Description = request.Description;
            schedule.StartDate = request.StartDate;
            schedule.EndDate = request.EndDate;
            schedule.ParentNotificationStartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            schedule.ParentNotificationEndDate = request.StartDate!.Value.AddDays(-1);
            schedule.CreatedBy = request.CreatedBy;
            schedule.UpdatedAt = DateTime.UtcNow;

            await healthCheckRepository.UpdateHealthCheckSchedule(schedule);
            return true;
        }

        public async Task<PaginationResponse<HealthCheckScheduleResponse>> GetPaginationHealthCheckSchedule(PaginationRequest? pagination)
        {
            var totalCount = await healthCheckRepository.CountAsync();

            var skip = (pagination!.PageIndex - 1) * (pagination!.PageSize);
            var schedules = await healthCheckRepository.GetPagedHealthCheckSchedule(skip, pagination.PageSize);
            if (schedules == null || !schedules.Any())
            {
                return null!;
            }
            List<HealthCheckScheduleResponse> healthCheckScheduleResponses = [];
            foreach (var schedule in schedules)
            {
                healthCheckScheduleResponses.Add(GetScheduleResponse(schedule));
            }
            return new PaginationResponse<HealthCheckScheduleResponse>(
                pagination!.PageIndex,
                pagination.PageSize,
                totalCount,
                healthCheckScheduleResponses
            );
        }

        public HealthCheckScheduleResponse GetScheduleResponse(HealthCheckSchedule schedule)
        {
            return new HealthCheckScheduleResponse
            {
                HealthCheckScheduleResponseDto = new HealthCheckScheduleResponseDto
                {
                    ScheduleId = schedule.ScheduleId,
                    Title = schedule.Title,
                    Description = schedule.Description,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    ParentNotificationStartDate = schedule.ParentNotificationStartDate,
                    ParentNotificationEndDate = schedule.ParentNotificationEndDate,
                    Status = schedule.Status,
                    CreatedAt = schedule.CreatedAt,
                    UpdatedAt = schedule.UpdatedAt
                }
            };
        }
    }
}
