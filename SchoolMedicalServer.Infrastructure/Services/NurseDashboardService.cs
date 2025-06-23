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
                    Count = totalAppointments,
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

            var pendingResults = filteredAppointments.Where(a => a.ConfirmationStatus == false).ToList();
            var confirmedResults = filteredAppointments.Where(a => a.ConfirmationStatus == true).ToList();
            var notCompletedResults = filteredAppointments.Where(a => a.ConfirmationStatus == true && a.CompletionStatus == false).ToList();
            var completedResults = filteredAppointments.Where(a => a.CompletionStatus == true && a.ConfirmationStatus == true).ToList();


            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {fromDate} to {toDate}",
                    Count = pendingResults.Count,
                    Details = pendingResults.Select(pending => new ItemDetais
                    {
                        Id = pending.AppointmentId,
                        Name = pending.AppointmentReason
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Confirmed in {fromDate} to {toDate}",
                    Count = confirmedResults.Count,
                    Details = confirmedResults.Select(confirmed => new  ItemDetais
                    {
                        Id = confirmed.AppointmentId,
                        Name = confirmed.AppointmentReason
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Not Completed in {fromDate} to {toDate}",
                    Count = notCompletedResults.Count,
                    Details = notCompletedResults.Select(notCompleted => new ItemDetais
                    {
                        Id = notCompleted.AppointmentId,
                        Name = notCompleted.AppointmentReason
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {fromDate} to {toDate}",
                    Count = completedResults.Count,
                    Details =  completedResults.Select(completed => new ItemDetais
                    {
                        Id = completed.AppointmentId,
                        Name = completed.AppointmentReason
                    }).ToList()
                }
            });
            return responses;
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

            var eventTypeGroups = filteredEvents
                .Where(e => !string.IsNullOrEmpty(e.EventType))
                .GroupBy(e => e.EventType!.Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => g.Key)
                .ToList();
            foreach (var group in eventTypeGroups)
            {
                responses.Add(new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"{group.Key} Events in {fromDate} to {toDate}",
                        Count = group.Count(),
                        Details = group.Select(Group => new ItemDetais
                        {
                            Id = Group.EventId,
                            Name = Group.SeverityLevel
                        }).ToList()
                    }
                });
            }

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

            var pendingRegistration = filteredRegistrations.Where(r => r.Status == false).ToList();
            var approvedRegistration = filteredRegistrations.Where(r => r.Status == true).ToList();
            var notCompletedRegistration = filteredRegistrations.Where(r => r.Status == true && r.Details.Any() && r.Details.Any(d => !d.IsCompleted)).ToList();
            var completedRegistration = filteredRegistrations.Where(r => r.Status == true && r.Details.Any() && r.Details.All(d => d.IsCompleted)).ToList();

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {fromDate} to {toDate}",
                    Count = pendingRegistration.Count ,
                    Details = pendingRegistration.Select(pending => new ItemDetais
                    {
                        Id = pending.RegistrationId ,
                        Name = pending.MedicationName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Approved in {fromDate} to {toDate}",
                    Count = approvedRegistration.Count ,
                    Details = approvedRegistration.Select(approved => new ItemDetais
                    {
                        Id = approved.RegistrationId ,
                        Name = approved.MedicationName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Not Completed in {fromDate} to {toDate}",
                    Count = notCompletedRegistration.Count ,
                    Details = notCompletedRegistration.Select(notCompleted => new ItemDetais
                    {
                        Id = notCompleted.RegistrationId ,
                        Name= notCompleted.MedicationName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {fromDate} to {toDate}",
                    Count = completedRegistration.Count ,
                    Details = completedRegistration.Select(completed => new ItemDetais
                    {
                        Id= completed.RegistrationId ,
                        Name= completed.MedicationName
                    }).ToList()
                }
            });
            return responses;
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
