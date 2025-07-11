using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;
using System.Globalization;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationRoundService(
        IVaccinationRoundRepository vaccinationRound,
        IVaccinationResultRepository vaccinationResultRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        IVaccinationScheduleRepository scheduleRepository,
        IHealthProfileRepository healthProfile) : IVaccinationRoundService
    {
        public async Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId)
        {
            var totalCount = await vaccinationResultRepository.CountByRoundIdAsync(roundId);
            if (totalCount == 0)
            {
                return null!;
            }
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var results = await vaccinationResultRepository.GetPagedStudents(roundId, pagination.Search!, skip, pagination.PageSize);
            List<VaccinationRoundStudentResponse> responses = new();
            foreach (var result in results)
            {
                var studentResponse = await StudentsOfRoundResponse(result!);
                var parentResponse = await ParentOfStudentResponse(result!);
                responses.Add(new VaccinationRoundStudentResponse
                {
                    StudentsOfRoundResponse = studentResponse,
                    ParentsOfStudent = parentResponse
                });
            }
            return new PaginationResponse<VaccinationRoundStudentResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                responses
            );
        }

        private async Task<StudentsOfRoundResponse> StudentsOfRoundResponse(VaccinationResult result)
        {
            var student = await studentRepository.GetStudentByIdAsync(result.HealthProfile!.StudentId);
            return new StudentsOfRoundResponse
            {
                StudentId = student?.StudentId,
                VaccinationResultId = result.VaccinationResultId,
                FullName = student?.FullName,
                StudentCode = student?.StudentCode,
                DayOfBirth = student?.DayOfBirth,
                Gender = student?.Gender,
                Grade = student?.Grade,
                ResultStatus = result.Status
            };
        }

        private async Task<ParentOfStudentResponse> ParentOfStudentResponse(VaccinationResult result)
        {
            var userId = await studentRepository.GetParentUserIdAsync(result.HealthProfile!.StudentId);
            var user = await userRepository.GetByIdAsync(userId);
            return new ParentOfStudentResponse
            {
                UserId = user!.UserId,
                FullName = user?.FullName,
                PhoneNumber = user!.PhoneNumber,
                ParentConfirm = result.ParentConfirmed
            };
        }

        public async Task<VaccinationRoundResponse> GetVaccinationRoundByIdAsync(Guid roundId)
        {
            var round = await vaccinationRound.GetVaccinationRoundByIdAsync(roundId);
            if (round == null)
            {
                return null!;
            }
            var nurse = await userRepository.GetByIdAsync(round.NurseId);
            if (nurse == null)
            {
                return null!;
            }
            return MapToRoundResponse(round, nurse);
        }

        public VaccinationRoundResponse MapToRoundResponse(VaccinationRound round, User nurse)
        {
            return new VaccinationRoundResponse
            {
                VaccinationRoundInformation = new VaccinationRoundInformationResponse
                {
                    RoundId = round.RoundId,
                    RoundName = round.RoundName!,
                    TargetGrade = round.TargetGrade!,
                    StartTime = round.StartTime,
                    EndTime = round.EndTime,
                    Description = round.Description!,
                    Status = round.Status
                },
                Nurse = new VaccinationRoundNurseInformationResponse
                {
                    NurseId = nurse!.UserId,
                    NurseName = nurse.FullName!,
                    PhoneNumber = nurse.PhoneNumber,
                    AvatarUrl = nurse.AvatarUrl!
                }
            };
        }

        public async Task<PaginationResponse<VaccinationRoundResponse>> GetVaccinationRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination)
        {
            var totalCount = await vaccinationRound.CountByNurseIdAsync(nurseId);
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var rounds = await vaccinationRound.GetVaccinationRoundsByNurseIdAsync(nurseId, pagination.Search!, skip, pagination.PageSize);
            if (rounds.Count() == 0)
            {
                return null!;
            }
            var response = new List<VaccinationRoundResponse>();
            foreach (var round in rounds)
            {
                var nurse = await userRepository.GetByIdAsync(round.NurseId);
                response.Add(MapToRoundResponse(round, nurse!));
            }
            return new PaginationResponse<VaccinationRoundResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                response
            );
        }

        public async Task<NotificationRequest> UpdateVaccinationRoundStatusAsync(Guid roundId, bool request)
        {
            var round = await vaccinationRound.GetVaccinationRoundByIdAsync(roundId);
            if (round == null)
            {
                return null!;
            }
            var today = DateTime.UtcNow;
            if (round.EndTime < today)
            {
                return null!;
            }
            round.Status = request;
            round.UpdatedAt = today;
            await vaccinationRound.UpdateVaccinationRound(round);

            var notifcation = new NotificationRequest
            {
                NotificationTypeId = round.RoundId,
                SenderId = round.NurseId,
                ReceiverId = round.Schedule!.CreatedBy,
            };

            return notifcation;
        }

        public async Task<IEnumerable<VaccinationRoundResponse>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId)
        {
            var rounds = await vaccinationRound.GetVaccinationRoundsByScheduleIdAsync(scheduleId);
            var response = new List<VaccinationRoundResponse>();
            foreach (var round in rounds)
            {
                var nurse = await userRepository.GetByIdAsync(round.NurseId);
                response.Add(MapToRoundResponse(round, nurse!));
            }
            return response;
        }

        public async Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId)
        {
            var round = await vaccinationRound.GetVaccinationRoundByIdAsync(roundId);
            if (round == null || round.NurseId != nurseId)
            {
                return null!;
            }
            var total = await vaccinationResultRepository.CountByRoundIdAsync(roundId);
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var results = await vaccinationResultRepository.GetPagedStudents(roundId, pagination.Search!, skip, pagination.PageSize);

            var confirmedResults = results
                      .Where(r => r != null && r.ParentConfirmed == true)
                      .ToList();

            List<VaccinationRoundStudentResponse> responses = new();
            foreach (var result in confirmedResults)
            {
                var studentResponse = await StudentsOfRoundResponse(result!);
                var parentResponse = await ParentOfStudentResponse(result!);
                responses.Add(new VaccinationRoundStudentResponse
                {
                    VaccineId = round.Schedule!.VaccineId,
                    StudentsOfRoundResponse = studentResponse,
                    ParentsOfStudent = parentResponse
                });
            }
            return new PaginationResponse<VaccinationRoundStudentResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                total,
                responses
            );

        }

        public async Task<bool> CreateVaccinationRegularRoundByScheduleIdAsync(VaccinationRoundRequest request)
        {
            var schedule = await scheduleRepository.GetVaccinationScheduleByIdAsync(request.ScheduleId!.Value);
            if (schedule == null)
            {
                return false;
            }
            if (schedule.Rounds.Any(r => r.TargetGrade!.Equals(request.TargetGrade!)))
                return false;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var round = new VaccinationRound
            {
                RoundId = Guid.NewGuid(),
                RoundName = textInfo.ToTitleCase(request.RoundName!.ToLower().Trim()),
                TargetGrade = request.TargetGrade,
                Description = request.Description,
                StartTime = request.StartTime!.Value,
                EndTime = request.EndTime!.Value,
                NurseId = request.NurseId,
                ScheduleId = schedule.ScheduleId,
                Status = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await vaccinationRound.CreateVaccinationRoundAsync(round);
            return true;
        }

        public async Task<IEnumerable<VaccinationRoundParentResponse>> GetVaccinationRoundsByUserIdAsync(Guid userId, DateOnly? start, DateOnly? end)
        {
            var students = await studentRepository.GetByParentIdAsync(userId);
            var healthProfiles = await healthProfile.GetByStudentIdsAsync(students.Select(s => s.StudentId));
            var vaccinationResults = await vaccinationResultRepository.GetByHealthProfileIdsAsync(healthProfiles.Select(h => h.HealthProfileId));

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = start ?? today.AddDays(-1 * diff);
            var weekEnd = end ?? weekStart.AddDays(7);

            List<VaccinationRoundParentResponse> responses = new();
            foreach (var result in vaccinationResults)
            {
                var round = await vaccinationRound.GetVaccinationRoundByIdAsync(result!.Round!.RoundId);
                if (round == null)
                {
                    continue;
                }
                if (DateOnly.FromDateTime((DateTime)round.StartTime!) >= weekStart &&
                    DateOnly.FromDateTime((DateTime)round.EndTime!) <= weekEnd)
                {
                    var nurse = await userRepository.GetByIdAsync(round.NurseId);
                    if (nurse == null)
                    {
                        continue;
                    }
                    responses.Add(await MapToParentReponseAsync(round, nurse, result));
                }
            }
            return responses;
        }

        private async Task<VaccinationRoundParentResponse> MapToParentReponseAsync(VaccinationRound round, User nurse, VaccinationResult result)
        {
            var res = new VaccinationRoundParentResponse();
            res.VaccinationRoundInformation = new VaccinationRoundInformationResponse
            {
                RoundId = round.RoundId,
                RoundName = round.RoundName!,
                TargetGrade = round.TargetGrade!,
                StartTime = round.StartTime,
                EndTime = round.EndTime,
                Description = round.Description!,
                Status = round.Status
            };
            res.Nurse = new VaccinationRoundNurseInformationResponse
            {
                NurseId = nurse!.UserId,
                NurseName = nurse.FullName!,
                PhoneNumber = nurse.PhoneNumber,
                AvatarUrl = nurse.AvatarUrl!
            };
            res.Student = await StudentsOfRoundResponse(result!);
            res.Parent = await ParentOfStudentResponse(result!);

            return res;
        }

        public async Task<bool> UpdateVaccinationRoundAsync(Guid roundId, VaccinationRoundUpdateRequest request)
        {
            var updateRound = await vaccinationRound.GetVaccinationRoundByIdAsync(roundId);
            if (updateRound == null)
            {
                return false;
            }
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            updateRound.RoundName = textInfo.ToTitleCase(request.RoundName!.ToLower().Trim());
            updateRound.TargetGrade = request.TargetGrade;
            updateRound.Description = request.Description;
            updateRound.StartTime = request.StartTime;
            updateRound.EndTime = request.EndTime;
            updateRound.NurseId = request.NurseId;
            updateRound.UpdatedAt = DateTime.UtcNow;
            await vaccinationRound.UpdateVaccinationRound(updateRound);
            return true;
        }

        public async Task<IEnumerable<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForNurseAsync(Guid roundId, Guid nurseId)
        {
            var round = await vaccinationRound.GetVaccinationRoundByIdAsync(roundId);
            if (round == null || round.NurseId != nurseId)
            {
                return null!;
            }
            var results = await vaccinationResultRepository.GetByRoundIdAsync(roundId);
            if (results == null || results.Count() == 0)
            {
                return null!;
            }
            var confirmedResults = results
                      .Where(r => r != null && r.ParentConfirmed == true)
                      .ToList();
            List<VaccinationRoundStudentResponse> responses = new();
            foreach (var result in confirmedResults)
            {
                var studentResponse = await StudentsOfRoundResponse(result!);
                var parentResponse = await ParentOfStudentResponse(result!);
                responses.Add(new VaccinationRoundStudentResponse
                {
                    StudentsOfRoundResponse = studentResponse,
                    ParentsOfStudent = parentResponse
                });
            }
            ;
            return responses;
        }

        public async Task<int> GetTotalSupplementaryTotalStudentsAsync(Guid scheduleId)
        {
            var schedule = await scheduleRepository.GetVaccinationScheduleByIdAsync(scheduleId);
            if (schedule == null)
            {
                return 0;
            }
            if (schedule.Rounds.Any(r => r.TargetGrade != null && r.TargetGrade.ToLower().Contains("supplement")))
            {
                return 0;
            }
            var supplementStudents = schedule.Rounds
                .Where(r => r.Status == true)
                .SelectMany(r => r.VaccinationResults)
                .Where(vr => vr.ParentConfirmed == true && vr.HealthQualified == true && vr.Vaccinated == false)
                .ToList();
            return supplementStudents.Count;
        }
    }
}
