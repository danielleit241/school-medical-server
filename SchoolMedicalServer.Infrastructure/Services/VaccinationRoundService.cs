using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Rounds;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationRoundService(
        IVaccinationRoundRepository vaccinationRound,
        IVaccinationResultRepository vaccinationResultRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository) : IVaccinationRoundService
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
                totalCount,
                pagination.PageIndex,
                pagination.PageSize,
                responses
            );
        }

        private async Task<StudentsOfRoundResponse> StudentsOfRoundResponse(VaccinationResult result)
        {
            var student = await studentRepository.GetStudentByIdAsync(result.HealthProfile!.StudentId);
            return new StudentsOfRoundResponse
            {
                StudentId = student?.StudentId,
                FullName = student?.FullName,
                StudentCode = student?.StudentCode,
                DayOfBirth = student?.DayOfBirth,
                Gender = student?.Gender,
                Grade = student?.Grade
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
                    StartDate = round.StartDate,
                    EndDate = round.EndDate,
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
                totalCount,
                pagination.PageIndex,
                pagination.PageSize,
                response
            );
        }

        public Task<bool> UpdateVaccinationRoundAsync(Guid roundId, VaccinationRoundRequest request)
        {
            throw new NotImplementedException();
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

            var totalCount = await vaccinationResultRepository.CountByRoundIdAsync(roundId);
            if (totalCount == 0)
            {
                return null!;
            }
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
                    StudentsOfRoundResponse = studentResponse,
                    ParentsOfStudent = parentResponse
                });
            }
            return new PaginationResponse<VaccinationRoundStudentResponse>(
                totalCount,
                pagination.PageIndex,
                pagination.PageSize,
                responses
            );

        }
    }
}
