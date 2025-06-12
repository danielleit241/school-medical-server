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
        IBaseRepository baseRepository,
        IVaccinationRoundRepository vaccinationRoundRepository,
        IVacctionDetailsRepository vacctionDetailsRepository,
        IVaccinationResultRepository resultRepository,
        IStudentRepository studentRepository,
        IHealthProfileRepository profileRepository) : IVaccinationScheduleService
    {
        public async Task<NotificationVaccinationResponse> CreateScheduleAsync(VaccinationScheduleRequest request)
        {
            if (request == null)
            {
                return null!;
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
                    StartDate = round.StartDate,
                    EndDate = round.EndDate,
                    NurseId = round.NurseId,
                })]
            };

            await vaccinationScheduleRepository.CreateVaccinationSchedule(vaccinationSchedule);
            await baseRepository.SaveChangesAsync();
            var notificationRequests = await CreateVaccinationResultsByRounds(vaccinationSchedule.Rounds, vaccinationSchedule.ScheduleId, vaccinationSchedule.CreatedBy);
            return notificationRequests;
        }

        private async Task<NotificationVaccinationResponse> CreateVaccinationResultsByRounds(ICollection<VaccinationRound> rounds, Guid scheduleId, Guid CreateBy)
        {
            var toParents = new List<NotificationRequest>();
            var toNurses = new List<NotificationRequest>();
            foreach (var round in rounds)
            {
                var students = await studentRepository.GetStudentsByGradeAsync(round.TargetGrade);
                foreach (var student in students)
                {
                    var healthProfile = await profileRepository.GetByStudentIdAsync(student.StudentId);
                    var result = new VaccinationResult
                    {
                        VaccinationResultId = Guid.NewGuid(),
                        HealthProfileId = healthProfile!.HealthProfileId,
                        RoundId = round.RoundId,
                        ParentConfirmed = false,
                        Vaccinated = false,
                        VaccinatedDate = null,
                        RecorderId = round.NurseId,
                        Notes = null
                    };
                    await resultRepository.Create(result);
                    toParents.Add(new NotificationRequest
                    {
                        NotificationTypeId = result.VaccinationResultId,
                        SenderId = CreateBy,
                        ReceiverId = student.UserId,
                    });
                }
                toNurses.Add(new NotificationRequest
                {
                    NotificationTypeId = scheduleId,
                    SenderId = CreateBy,
                    ReceiverId = round.NurseId,
                });
            }
            await baseRepository.SaveChangesAsync();
            return new NotificationVaccinationResponse(toParents, toNurses);
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
                var vaccinationRounds = await vaccinationRoundRepository.GetVaccinationRoundsByScheduleIdAsync(schedule.ScheduleId);
                var vaccinationDetails = await vacctionDetailsRepository.GetByIdAsync(schedule.VaccineId);
                vaccinationScheduleResponses.Add(GetResponse(schedule, vaccinationRounds, vaccinationDetails));
            }
            return new PaginationResponse<VaccinationScheduleResponse?>(
                pagination!.PageIndex,
                pagination.PageSize,
                totalCount,
                vaccinationScheduleResponses
            );
        }

        public async Task<VaccinationScheduleResponse?> GetVaccinationSchedule(Guid id)
        {
            var schedule = await vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(id);
            if (schedule == null)
            {
                return null;
            }
            var vaccinationRounds = await vaccinationRoundRepository.GetVaccinationRoundsByScheduleIdAsync(schedule.ScheduleId);
            var vaccinationDetails = await vacctionDetailsRepository.GetByIdAsync(schedule.VaccineId);
            return GetResponse(schedule, vaccinationRounds, vaccinationDetails);
        }

        public VaccinationScheduleResponse GetResponse(VaccinationSchedule schedule, IEnumerable<VaccinationRound> rounds, VaccinationDetail? vaccinationDetails)
        {
            return new VaccinationScheduleResponse
            {
                VaccinationScheduleResponseDto = new VaccinationScheduleResponseDto
                {
                    ScheduleId = schedule.ScheduleId,
                    Title = schedule.Title,
                    Description = schedule.Description,
                    CreatedAt = schedule.CreatedAt,
                    UpdatedAt = schedule.UpdatedAt
                },
                VaccinationRounds = [.. rounds.Select(round => new VaccinationRoundResponseDto
                    {
                        RoundId = round.RoundId,
                        RoundName = round.RoundName,
                        TargetGrade = round.TargetGrade,
                        Description = round.Description,
                        StartDate = round.StartDate,
                        EndDate = round.EndDate,
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
    }
}
