using System.Threading.Tasks;
using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Results;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckResultService(
        IHealthCheckResultRepository healthCheckResultRepository,
        IHealthCheckRoundRepository healthCheckRoundRepository,
        IHealthCheckScheduleRepository healthCheckScheduleRepository,
        IUserRepository userRepository) : IHealthCheckResultService
    {
        public async Task<bool?> ConfirmOrDeclineHealthCheck(Guid resultId, ParentHealthCheckConfirmationRequest request)
        {
            var result = await healthCheckResultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return false;
            }
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(result.RoundId);
            if (round == null)
            {
                return false;
            }
            var schedule = await healthCheckScheduleRepository.GetHealthCheckScheduleByIdAsync(round.ScheduleId);
            if (schedule == null)
            {
                return false;
            }
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (today < schedule.ParentNotificationStartDate || today > schedule.ParentNotificationEndDate)
            {
                return false;
            }
            result.ParentConfirmed = request.Status;
            if (result.ParentConfirmed == false)
            {
                result.Notes = "Parent declined health check.";
                result.Status = "Failed";
            }
            await healthCheckResultRepository.UpdateAsync(result);
            return result.ParentConfirmed;
        }

        public async Task<bool> CreateHealthCheckResultAsync(HealthCheckResultRequest request)
        {
            if (request == null)
            {
                return false!;
            }
            var result = await healthCheckResultRepository.GetByIdAsync(request.HealthCheckResultId);
            if (result == null)
            {
                return false;
            }
            var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(result.RoundId);
            if (round == null)
            {
                return false;
            }
            result.DatePerformed = request.DatePerformed ?? DateOnly.FromDateTime(DateTime.UtcNow);
            result.Notes = request.Notes;
            result.Height = request.Height;
            result.Weight = request.Weight;
            result.VisionLeft = request.VisionLeft;
            result.VisionRight = request.VisionRight;
            result.Hearing = request.Hearing;
            result.Nose = request.Nose;
            result.BloodPressure = request.BloodPressure;
            result.Status = request.Status;
            result.RecordedId = round.NurseId;
            result.RecordedAt = DateTime.UtcNow;
            await healthCheckResultRepository.UpdateAsync(result);
            return true;
        }

        public async Task<HealthCheckResultResponse> GetHealthCheckResultAsync(Guid resultId)
        {
            var result = await healthCheckResultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return null!;
            }
            var res = await MapToResultRespone(result);
            return res;
        }

        private async Task<HealthCheckResultResponse> MapToResultRespone(HealthCheckResult result)
        {
            return new HealthCheckResultResponse
            {
                ResultId = result.ResultId,
                HealthProfileId = result.HealthProfileId,
                DatePerformed = result.DatePerformed,
                Height = result.Height,
                Weight = result.Weight,
                VisionLeft = result.VisionLeft,
                VisionRight = result.VisionRight,
                Hearing = result.Hearing,
                Nose = result.Nose,
                BloodPressure = result.BloodPressure,
                Status = result.Status,
                Notes = result.Notes,
                RecordedBy = await MapToNurseInforResponeAsync(result)
            };
        }

        private async Task<NurseInformationResponse> MapToNurseInforResponeAsync(HealthCheckResult result)
        {
            var user = await userRepository.GetByIdAsync(result.RecordedId);
            if (user == null)
            {
                return null!;
            }
            return new NurseInformationResponse
            {
                NurseId = result.RoundId,
                NurseName = user.FullName,
                NursePhone = user.PhoneNumber,
            };
        }

        public async Task<bool?> IsHealthCheckConfirmed(Guid resultId)
        {
            var result = await healthCheckResultRepository.GetByIdAsync(resultId);
            if (result == null)
            {
                return null;
            }
            return result.ParentConfirmed;
        }
    }
}
