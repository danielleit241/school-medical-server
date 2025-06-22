using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NurseDashboardService(
        IAppointmentRepository appointmentRepository,
        IMedicalEventRepository medicalEventRepository,
        IMedicalRegistrationRepository medicalRegistrationRepository,
        IHealthCheckScheduleRepository healthCheckScheduleRepository,
        IVaccinationScheduleRepository vaccinationScheduleRepository) : INurseDashboardService
    {
        public Task<int> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DashboardRoundResponse>> GetNurseHealthChecksDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            var (today, endOfWeek) = GetWeekRange();
            var schedules = await healthCheckScheduleRepository.GetHealthCheckSchedulesAsync();
            var rounds = schedules.SelectMany(s => s.Rounds).ToList();
            rounds = [.. rounds.Where(r => r.NurseId == nurseId && r.StartTime >= today && r.StartTime <= endOfWeek)];

            var responses = new List<DashboardRoundResponse>();
            foreach (var round in rounds)
            {
                responses.Add(new DashboardRoundResponse
                {
                    RoundName = round.RoundName,
                    Daylefts = ((DateTime)round.StartTime!).Date.Subtract(today).Days,
                    StartDate = DateOnly.FromDateTime((DateTime)round.StartTime!),
                });
            }
            return responses;
        }

        public Task<int> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DashboardRoundResponse>> GetNurseVaccinationsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            var (today, endOfWeek) = GetWeekRange();
            var schedules = await vaccinationScheduleRepository.GetVaccinationSchedulesAsync();
            var rounds = schedules.SelectMany(s => s.Rounds).ToList();
            rounds = [.. rounds.Where(r => r.NurseId == nurseId && r.StartTime >= today && r.StartTime <= endOfWeek)];

            var responses = new List<DashboardRoundResponse>();
            foreach (var round in rounds)
            {
                responses.Add(new DashboardRoundResponse
                {
                    RoundName = round.RoundName,
                    Daylefts = ((DateTime)round.StartTime!).Date.Subtract(today).Days,
                    StartDate = DateOnly.FromDateTime((DateTime)round.StartTime!),
                });
            }
            return responses;
        }

        private (DateTime today, DateTime endOfWeek) GetWeekRange()
        {
            var today = DateTime.UtcNow.Date;
            var daysUntilSunday = DayOfWeek.Sunday - today.DayOfWeek;
            if (daysUntilSunday <= 0) daysUntilSunday += 7;
            var endOfWeek = today.AddDays(daysUntilSunday);
            return (today, endOfWeek);
        }
    }
}
