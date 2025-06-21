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
        public async Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var allAppointments = new List<SchoolMedicalServer.Abstractions.Entities.Appointment>();

            if (fromDate != null && toDate != null)
            {
                for (var date = fromDate.Value; date <= toDate.Value; date = date.AddDays(1))
                {
                    var appointments = await appointmentRepository.GetByStaffNurseAndDateAsync(nurseId, date);
                    if (appointments != null)
                        allAppointments.AddRange(appointments);
                }
            }

            var totalAppointments = allAppointments.Count;

            return new List<DashboardResponse>
            {
                new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Total Appointments in {fromDate} to {toDate}",
                        Count = totalAppointments
                    }
                }
            };
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));
            var responses = new List<DashboardResponse>();

            var allAppointments = new List<SchoolMedicalServer.Abstractions.Entities.Appointment>();
            if (request.From != null && request.To != null)
            {
                for (var date = request.From.Value; date <= request.To.Value; date = date.AddDays(1))
                {
                    var appointments = await appointmentRepository.GetByStaffNurseAndDateAsync(nurseId, date);
                    if (appointments != null)
                        allAppointments.AddRange(appointments);
                }
            }

            allAppointments = [.. allAppointments.Where(a =>
            a.AppointmentDate >= request.From && a.AppointmentDate <= request.To)];

            var confirmedResults = allAppointments.Count(a => a.ConfirmationStatus == true);
            var pendingResults = allAppointments.Count(a => a.ConfirmationStatus == false);
            var completedResults = allAppointments.Count(a => a.CompletionStatus == true);
       

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Confirmed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = confirmedResults
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = pendingResults
                }
            });
           
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = completedResults
                }
            });
      

            return responses;
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var medicalEvents = await medicalEventRepository.GetPagedAsync(0, int.MaxValue);

            medicalEvents = [.. medicalEvents.Where(e =>
                e.StaffNurseId == nurseId &&
                e.EventDate >= fromDate &&
                e.EventDate <= toDate
            )];

            var totalMedicalEvents = medicalEvents.Count;

            return
            [
                new DashboardResponse
        {
            Item = new Item
            {
                Name = $"Total Medical Events in {fromDate} to {toDate}",
                Count = totalMedicalEvents
            }
        }
            ];
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;
            var responses = new List<DashboardResponse>();

            var medicalEvents = await medicalEventRepository.GetPagedAsync(0, int.MaxValue);

            medicalEvents = [.. medicalEvents.Where(e =>
                    e.StaffNurseId == nurseId &&
                    e.EventDate >= fromDate &&
                    e.EventDate <= toDate
                )];

            var LowEvents = medicalEvents.Where(e => e.SeverityLevel != null && e.SeverityLevel.ToLower().Contains("Low")).Count();
            var MediumEvents = medicalEvents.Where(e => e.SeverityLevel != null && e.SeverityLevel.ToLower().Contains("Medium")).Count();
            var HighEvents = medicalEvents.Where(e => e.SeverityLevel != null && e.SeverityLevel.ToLower().Contains("High")).Count();
           
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Low Events in {fromDate} to {toDate}",
                    Count = LowEvents
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Medium Events in {fromDate} to {toDate}",
                    Count = MediumEvents
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"High Events in {fromDate} to {toDate}",
                    Count = HighEvents
                }
            });
            

            return responses;
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var medicalRegistrations = await medicalRegistrationRepository.GetNursePagedAsync(nurseId, 0, int.MaxValue);

            medicalRegistrations = [.. medicalRegistrations.Where(r =>
                    r.DateSubmitted >= fromDate &&
                    r.DateSubmitted <= toDate
                )];

            var totalRegistrations = medicalRegistrations.Count;

            return
            [
                new DashboardResponse
        {
            Item = new Item
            {
                Name = $"Total Medical Registrations in {fromDate} to {toDate}",
                Count = totalRegistrations
            }
        }
            ];
        }

        public async Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var responses = new List<DashboardResponse>();
            var medicalRegistrations = await medicalRegistrationRepository.GetNursePagedAsync(nurseId, 0, int.MaxValue);

            medicalRegistrations = [.. medicalRegistrations.Where(r =>
                    r.DateSubmitted >= fromDate &&
                    r.DateSubmitted <= toDate
                )];

            
            var completedRegistrations = medicalRegistrations.Where(r => r.Status == true ).Count();
            var NotYetRegistrations = medicalRegistrations.Where(r => r.Status == false).Count();

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
                    Count = NotYetRegistrations
                }
            });


            return responses;
        }
    }
}
