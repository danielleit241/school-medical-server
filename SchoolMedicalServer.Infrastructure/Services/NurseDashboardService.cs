using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Repositories;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NurseDashboardService(
        IAppointmentRepository appointmentRepository,
        IMedicalEventRepository medicalEventRepository,
        IMedicalRegistrationRepository medicalRegistrationRepository) : INurseDashboardService
    {
        public async Task<DashboardResponse> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var allAppointments = await appointmentRepository.GetAllAppointment();
            var filteredAppointments = allAppointments
                .Where(a => a.StaffNurseId == nurseId)
                .Where(a =>
                    (!fromDate.HasValue || (a.AppointmentDate.HasValue && a.AppointmentDate.Value >= fromDate.Value)) &&
                    (!toDate.HasValue || (a.AppointmentDate.HasValue && a.AppointmentDate.Value <= toDate.Value))
                )
                .ToList();

            var totalAppointments = filteredAppointments.Count;

            return new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Total Appointments in {fromDate} to {toDate}",
                    Count = totalAppointments
                }
            };
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;
            var responses = new List<DashboardResponse>();

            var allAppointments = await appointmentRepository.GetAllAppointment();

            var filteredAppointments = allAppointments
                .Where(a => a.StaffNurseId == nurseId)
                .Where(a =>
                    (!fromDate.HasValue || (a.AppointmentDate.HasValue && a.AppointmentDate.Value >= fromDate.Value)) &&
                    (!toDate.HasValue || (a.AppointmentDate.HasValue && a.AppointmentDate.Value <= toDate.Value))
                )
                .ToList();

            var confirmedResults = filteredAppointments.Count(a => a.ConfirmationStatus == true);
            var pendingResults = filteredAppointments.Count(a => a.ConfirmationStatus == false);
            var completedResults = filteredAppointments.Count(a => a.CompletionStatus == true);

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Confirmed in {fromDate} to {toDate}",
                    Count = confirmedResults
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {fromDate} to {toDate}",
                    Count = pendingResults
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {fromDate} to {toDate}",
                    Count = completedResults
                }
            });

            return responses;
        }

        public async Task<DashboardResponse> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var medicalEvents = await medicalEventRepository.GetAllMedicalEvent();

            var filteredEvents = medicalEvents
                .Where(e => e.StaffNurseId == nurseId)
                .Where(e =>
                    (!fromDate.HasValue || (e.EventDate.HasValue && e.EventDate.Value >= fromDate.Value)) &&
                    (!toDate.HasValue || (e.EventDate.HasValue && e.EventDate.Value <= toDate.Value))
                )
                .ToList();

            var totalMedicalEvents = filteredEvents.Count;

            return new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Total Medical Events in {fromDate} to {toDate}",
                    Count = totalMedicalEvents
                }
            };
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;
            var responses = new List<DashboardResponse>();
            var medicalEvents = await medicalEventRepository.GetAllMedicalEvent();

            var filteredEvents = medicalEvents
                .Where(e => e.StaffNurseId == nurseId)
                .Where(e =>
                    (!fromDate.HasValue || (e.EventDate.HasValue && e.EventDate.Value >= fromDate.Value)) &&
                    (!toDate.HasValue || (e.EventDate.HasValue && e.EventDate.Value <= toDate.Value))
                )
                .ToList();

            var lowEvents = filteredEvents.Count(e => !string.IsNullOrEmpty(e.SeverityLevel) && e.SeverityLevel.Equals("Low", StringComparison.OrdinalIgnoreCase));
            var mediumEvents = filteredEvents.Count(e => !string.IsNullOrEmpty(e.SeverityLevel) && e.SeverityLevel.Equals("Medium", StringComparison.OrdinalIgnoreCase));
            var highEvents = filteredEvents.Count(e => !string.IsNullOrEmpty(e.SeverityLevel) && e.SeverityLevel.Equals("High", StringComparison.OrdinalIgnoreCase));

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Low Events in {fromDate} to {toDate}",
                    Count = lowEvents
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Medium Events in {fromDate} to {toDate}",
                    Count = mediumEvents
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"High Events in {fromDate} to {toDate}",
                    Count = highEvents
                }
            });

            return responses;
        }

        public async Task<DashboardResponse> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var medicalRegistrations = await medicalRegistrationRepository.GetAllMedicalRegistration();
            var filteredRegistrations = medicalRegistrations
                .Where(r => r.StaffNurseId == nurseId)
                .Where(r =>
                    (!fromDate.HasValue || (r.DateSubmitted.HasValue && r.DateSubmitted.Value >= fromDate.Value)) &&
                    (!toDate.HasValue || (r.DateSubmitted.HasValue && r.DateSubmitted.Value <= toDate.Value))
                )
                .ToList();

            var totalRegistrations = filteredRegistrations.Count;

            return new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Total Medical Registrations in {fromDate} to {toDate}",
                    Count = totalRegistrations
                }
            };
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var responses = new List<DashboardResponse>();

            var medicalRegistrations = await medicalRegistrationRepository.GetAllMedicalRegistration();
            var filteredRegistrations = medicalRegistrations
                .Where(r => r.StaffNurseId == nurseId)
                .Where(r =>
                    (!fromDate.HasValue || (r.DateSubmitted.HasValue && r.DateSubmitted.Value >= fromDate.Value)) &&
                    (!toDate.HasValue || (r.DateSubmitted.HasValue && r.DateSubmitted.Value <= toDate.Value))
                )
                .ToList();

            var completedRegistrations = filteredRegistrations.Count(r => r.Status == true);
            var notYetRegistrations = filteredRegistrations.Count(r => r.Status == false);

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {fromDate} to {toDate}",
                    Count = completedRegistrations
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Not Yet in {fromDate} to {toDate}",
                    Count = notYetRegistrations
                }
            });

            return responses;
        }
    }
}
