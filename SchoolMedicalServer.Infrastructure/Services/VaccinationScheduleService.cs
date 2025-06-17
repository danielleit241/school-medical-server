using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationScheduleService(
        IVaccinationScheduleRepository vaccinationScheduleRepository,
        IVaccinationRoundRepository vaccinationRoundRepository,
        IVaccinationDetailsRepository vacctionDetailsRepository,
        IVaccinationResultRepository resultRepository,
        IStudentRepository studentRepository,
        IHealthProfileRepository profileRepository) : IVaccinationScheduleService
    {
        public async Task<bool> CreateScheduleAsync(VaccinationScheduleRequest request)
        {
            if (request == null)
            {
                return false;
            }

            var vaccinationSchedule = new VaccinationSchedule
            {
                ScheduleId = Guid.NewGuid(),
                VaccineId = request.VaccineId,
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ParentNotificationStartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ParentNotificationEndDate = request.StartDate!.Value.AddDays(-1),
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Rounds = [.. request.VaccinationRounds.Select(round => new VaccinationRound
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

            await vaccinationScheduleRepository.CreateVaccinationSchedule(vaccinationSchedule);
            return true;
        }

        public async Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId)
        {
            var schedule = await vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(scheduleId);
            if (schedule == null)
                return null!;
            var toParents = new List<NotificationRequest>();
            var toNurses = new List<NotificationRequest>();
            foreach (var round in schedule!.Rounds)
            {
                if (await resultRepository.IsExistStudentByRoundId(round.RoundId))
                    continue;
                var students = await studentRepository.GetStudentsByGradeAsync(round.TargetGrade);
                foreach (var student in students)
                {
                    var healthProfile = await profileRepository.GetByStudentIdAsync(student.StudentId);
                    var result = new VaccinationResult
                    {
                        VaccinationResultId = Guid.NewGuid(),
                        HealthProfileId = healthProfile!.HealthProfileId,
                        RoundId = round.RoundId,
                        Vaccinated = false,
                        VaccinatedDate = null,
                        RecorderId = round.NurseId,
                        Notes = null,
                        Status = "Pending",
                    };
                    await resultRepository.Create(result);
                    toParents.Add(new NotificationRequest
                    {
                        NotificationTypeId = result.VaccinationResultId,
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
            return new NotificationScheduleResponse(toParents, toNurses);
        }

        public async Task<PaginationResponse<VaccinationScheduleResponse?>?> GetPaginationVaccinationSchedule(PaginationRequest? pagination)
        {
            var totalCount = await vaccinationScheduleRepository.CountAsync();
            var skip = (pagination!.PageIndex - 1) * (pagination!.PageSize);
            var schedules = await vaccinationScheduleRepository.GetPagedVaccinationSchedule(skip, pagination.PageSize);
            if (schedules == null || !schedules.Any())
            {
                return null;
            }
            List<VaccinationScheduleResponse> vaccinationScheduleResponses = [];
            foreach (var schedule in schedules)
            {
                vaccinationScheduleResponses.Add(GetScheduleResponse(schedule));
            }
            return new PaginationResponse<VaccinationScheduleResponse?>(
                pagination!.PageIndex,
                pagination.PageSize,
                totalCount,
                vaccinationScheduleResponses
            );
        }

        public async Task<VaccinationScheduleDetailsResponse?> GetVaccinationSchedule(Guid id)
        {
            var schedule = await vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(id);
            if (schedule == null)
            {
                return null;
            }
            var vaccinationRounds = await vaccinationRoundRepository.GetVaccinationRoundsByScheduleIdAsync(schedule.ScheduleId);
            var vaccinationDetails = await vacctionDetailsRepository.GetByIdAsync(schedule.VaccineId);
            return GetScheduleDetailsResponse(vaccinationRounds, vaccinationDetails);
        }

        public VaccinationScheduleResponse GetScheduleResponse(VaccinationSchedule schedule)
        {
            return new VaccinationScheduleResponse
            {
                VaccinationScheduleResponseDto = new VaccinationScheduleResponseDto
                {
                    ScheduleId = schedule.ScheduleId,
                    VaccineId = schedule.VaccineId,
                    Title = schedule.Title,
                    Description = schedule.Description,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    ParentNotificationStartDate = schedule.ParentNotificationStartDate,
                    ParentNotificationEndDate = schedule.ParentNotificationEndDate,
                    CreatedAt = schedule.CreatedAt,
                    UpdatedAt = schedule.UpdatedAt
                }
            };
        }

        public VaccinationScheduleDetailsResponse GetScheduleDetailsResponse(IEnumerable<VaccinationRound> rounds, VaccinationDetail? vaccinationDetails)
        {
            return new VaccinationScheduleDetailsResponse
            {
                VaccinationRounds = [.. rounds.Select(round => new VaccinationRoundResponseDto
                    {
                        RoundId = round.RoundId,
                        RoundName = round.RoundName,
                        TargetGrade = round.TargetGrade,
                        Description = round.Description,
                        StartTime = round.StartTime,
                        EndTime = round.EndTime,
                        NurseId = round.NurseId,
                        Status = round.Status
                    })],
                VaccinationDetailsResponse = new VaccinationDetailsResponse
                {
                    VaccineId = vaccinationDetails!.VaccineId,
                    VaccineCode = vaccinationDetails.VaccineCode,
                    VaccineName = vaccinationDetails.VaccineName,
                    VaccineType = vaccinationDetails.VaccineType,
                    AgeRecommendation = vaccinationDetails.AgeRecommendation,
                    BatchNumber = vaccinationDetails.BatchNumber,
                    ContraindicationNotes = vaccinationDetails.ContraindicationNotes,
                    Description = vaccinationDetails.Description,
                    ExpirationDate = vaccinationDetails.ExpirationDate,
                    Manufacturer = vaccinationDetails.Manufacturer,
                }
            };
        }

        public async Task<bool> UpdateScheduleAsync(Guid scheduleId, VaccinationScheduleUpdateRequest request)
        {
            var schedule = await vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(scheduleId);
            if (schedule == null)
                return false;

            foreach (var round in schedule.Rounds)
            {
                if (await resultRepository.IsExistStudentByRoundId(round.RoundId))
                    return false;
            }

            schedule.VaccineId = request.VaccineId;
            schedule.Title = request.Title;
            schedule.Description = request.Description;
            schedule.StartDate = request.StartDate;
            schedule.EndDate = request.EndDate;
            schedule.ParentNotificationStartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            schedule.ParentNotificationEndDate = request.StartDate!.Value.AddDays(-1);
            schedule.CreatedBy = request.CreatedBy;
            schedule.UpdatedAt = DateTime.UtcNow;

            vaccinationScheduleRepository.UpdateVaccinationSchedule(schedule);
            return true;
        }
    }
}
