using SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckRoundService(
        IHealthCheckRoundRepository healthCheckRoundRepository,
        IHealthCheckScheduleRepository healthCheckScheduleRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        IHealthCheckResultRepository healthCheckResultRepository,
        IHealthProfileRepository profileRepository) : IHealthCheckRoundService
    {
        public async Task<bool> CreateHealthCheckRoundByScheduleIdAsync(HealthCheckRoundRequest request)
        {
            if (request == null || request.ScheduleId == Guid.Empty)
            {
                return false;
            }
            var schedule = await healthCheckScheduleRepository.GetHealthCheckScheduleByIdAsync(request.ScheduleId);
            if (schedule == null)
            {
                return false;
            }
            if (schedule.Rounds.Any(r => r.TargetGrade!.Equals(request.TargetGrade!)))
                return false;

            var round = new HealthCheckRound
            {
                RoundId = Guid.NewGuid(),
                RoundName = request.RoundName,
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
            await healthCheckRoundRepository.CreateHealthCheckRoundAsync(round);
            return true;
        }

        public async Task<PaginationResponse<HealthCheckRoundResponse>> GetHealthCheckRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination)
        {
            var totalCount = await healthCheckRoundRepository.CountByNurseIdAsync(nurseId);
            if (totalCount == 0)
            {
                return null!;
            }
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var rounds = await healthCheckRoundRepository.GetHealthCheckRoundsByNurseIdAsync(nurseId, pagination.Search!, skip, pagination.PageSize);
            List<HealthCheckRoundResponse> responses = [];
            foreach (var round in rounds)
            {
                responses.Add(await MapToDetailsResponse(round));
            }
            return new PaginationResponse<HealthCheckRoundResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                responses
            );
        }

        private async Task<HealthCheckRoundResponse> MapToDetailsResponse(HealthCheckRound round)
        {
            var nurseInfor = await userRepository.GetByIdAsync(round.NurseId);
            return new HealthCheckRoundResponse
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

        public async Task<IEnumerable<HealthCheckRoundParentResponse>> GetHealthCheckRoundsByUserIdAsync(Guid userId, DateOnly? start, DateOnly? end)
        {
            var students = await studentRepository.GetByParentIdAsync(userId);
            var healthProfiles = await profileRepository.GetByStudentIdsAsync(students.Select(s => s.StudentId).ToList());
            var results = await healthCheckResultRepository.GetByHealthProfileIdsAsync(healthProfiles.Select(h => h.HealthProfileId));

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = start ?? today.AddDays(-1 * diff);
            var weekEnd = end ?? weekStart.AddDays(7);

            List<HealthCheckRoundParentResponse> responses = new();
            foreach (var result in results)
            {
                var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(result!.Round!.RoundId);
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

        private async Task<HealthCheckRoundParentResponse> MapToParentReponseAsync(HealthCheckRound round, User nurse, HealthCheckResult result)
        {
            var res = new HealthCheckRoundParentResponse();
            res.HealthCheckRoundInformationResponse = new HealthCheckRoundInformationResponse
            {
                RoundId = round.RoundId,
                RoundName = round.RoundName!,
                TargetGrade = round.TargetGrade!,
                StartTime = round.StartTime,
                EndTime = round.EndTime,
                Description = round.Description!,
                Status = round.Status
            };
            res.HealthCheckRoundNurseInformationResponse = new HealthCheckRoundNurseInformationResponse
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

        public async Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId)
        {
            var totalCount = await healthCheckResultRepository.CountByRoundIdAsync(roundId);
            if (totalCount == 0)
            {
                return null!;
            }
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var results = await healthCheckResultRepository.GetPagedStudents(roundId, pagination.Search!, skip, pagination.PageSize);

            List<HealthCheckRoundStudentResponse> responses = new();
            foreach (var result in results)
            {
                var studentResponse = await StudentsOfRoundResponse(result!);
                var parentResponse = await ParentOfStudentResponse(result!);
                responses.Add(new HealthCheckRoundStudentResponse
                {
                    StudentsOfRoundResponse = studentResponse,
                    ParentOfStudent = parentResponse
                });
            }
            return new PaginationResponse<HealthCheckRoundStudentResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                responses
            );
        }

        public async Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId)
        {
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(roundId);
            if (round == null || round.NurseId != nurseId)
            {
                return null!;
            }
            var totalCount = await healthCheckResultRepository.CountByRoundIdAsync(roundId);
            if (totalCount == 0)
            {
                return null!;
            }
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var results = await healthCheckResultRepository.GetPagedStudents(roundId, pagination.Search!, skip, pagination.PageSize);
            var nurseResults = results.Where(r => r!.ParentConfirmed == true).ToList();

            List<HealthCheckRoundStudentResponse> responses = [];
            foreach (var result in nurseResults)
            {
                var studentResponse = await StudentsOfRoundResponse(result!);
                var parentResponse = await ParentOfStudentResponse(result!);
                responses.Add(new HealthCheckRoundStudentResponse
                {
                    StudentsOfRoundResponse = studentResponse,
                    ParentOfStudent = parentResponse
                });
            }
            return new PaginationResponse<HealthCheckRoundStudentResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                responses
            );
        }

        public async Task<bool> UpdateHealthCheckRoundStatusAsync(Guid roundId, bool request)
        {
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(roundId);
            if (round == null)
            {
                return false;
            }
            var today = DateTime.UtcNow;
            if (round.EndTime < today)
            {
                return false;
            }
            round.Status = request;
            round.UpdatedAt = DateTime.UtcNow;
            await healthCheckRoundRepository.UpdateHealthCheckRound(round);
            return true;
        }

        private async Task<StudentsOfRoundResponse> StudentsOfRoundResponse(HealthCheckResult result)
        {
            var student = await studentRepository.GetStudentByIdAsync(result.HealthProfile!.StudentId);
            return new StudentsOfRoundResponse
            {
                StudentId = student?.StudentId,
                HealthCheckResultId = result.ResultId,
                FullName = student?.FullName,
                StudentCode = student?.StudentCode,
                DayOfBirth = student?.DayOfBirth,
                Gender = student?.Gender,
                Grade = student?.Grade
            };
        }

        private async Task<ParentOfStudentResponse> ParentOfStudentResponse(HealthCheckResult result)
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

        public async Task<bool> UpdateHealthCheckRoundAsync(Guid roundId, HealthCheckRoundUpdateRequest request)
        {
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(roundId);
            if (round == null || request == null)
            {
                return false;
            }
            round.RoundName = request.RoundName;
            round.TargetGrade = request.TargetGrade;
            round.Description = request.Description;
            round.StartTime = request.StartTime;
            round.EndTime = request.EndTime;
            round.NurseId = request.NurseId;
            round.UpdatedAt = DateTime.UtcNow;
            await healthCheckRoundRepository.UpdateHealthCheckRound(round);
            return true;
        }

        public async Task<HealthCheckRoundResponse> GetHealthCheckRoundByIdAsync(Guid roundId)
        {
            if (roundId == Guid.Empty)
                return null!;
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(roundId);
            if (round is null)
                return null!;
            var res = await MapToDetailsResponse(round);
            return res;
        }

        public async Task<IEnumerable<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForNurseAsync(Guid roundId, Guid nurseId)
        {
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(roundId);
            if (round == null || round.NurseId != nurseId)
            {
                return null!;
            }
            var results = await healthCheckResultRepository.GetByRoundIdAsync(roundId);
            if (results == null || !results.Any())
            {
                return null!;
            }

            List<HealthCheckRoundStudentResponse> responses = [];
            foreach (var result in results)
            {
                var studentResponse = await StudentsOfRoundResponse(result!);
                var parentResponse = await ParentOfStudentResponse(result!);
                responses.Add(new HealthCheckRoundStudentResponse
                {
                    StudentsOfRoundResponse = studentResponse,
                    ParentOfStudent = parentResponse
                });
            }
            return responses;
        }
    }
}
