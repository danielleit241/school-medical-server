using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AppointmentService(SchoolMedicalManagementContext context) : IAppointmentService
    {
        public async Task<IEnumerable<StaffNurseDto>> GetStaffNurses()
        {
            var staffNurses = await context.Users.Include(u => u.Role).Where(u => u.Role!.RoleName == "nurse").Where(u => u.Status == true).ToListAsync();

            if (staffNurses == null || staffNurses.Count == 0)
            {
                return [];
            }

            var response = new List<StaffNurseDto>();
            foreach (var staffNurse in staffNurses)
            {
                response.Add(new StaffNurseDto
                {
                    Id = staffNurse.UserId,
                    FullName = staffNurse.FullName,
                    PhoneNumber = staffNurse.PhoneNumber,
                });
            }

            return response;
        }

        public async Task<IEnumerable<AppointmentResponse>?> GetAppointmentsByStaffNurseAndDate(Guid staffNurseId, DateOnly? dateRequest)
        {
            if (staffNurseId == Guid.Empty)
            {
                return [];
            }

            var date = dateRequest.HasValue ? dateRequest : DateOnly.FromDateTime(DateTime.UtcNow.Date);

            var appointments = await context.Appointments
                                            .Include(u => u.User)
                                            .Include(s => s.Student)
                                            .Where(a => a.AppointmentDate == date && a.StaffNurseId == staffNurseId)
                                            .ToListAsync();
            if (appointments == null || appointments.Count == 0)
                return [];

            var response = new List<AppointmentResponse>();
            foreach (var appointment in appointments)
            {
                response.Add(new AppointmentResponse
                {
                    Student = new StudentInfo
                    {
                        StudentId = appointment.Student!.StudentId,
                        StudentCode = appointment.Student.StudentCode,
                        FullName = appointment.Student.FullName
                    },
                    User = new UserInfo
                    {
                        UserId = appointment.User!.UserId,
                        FullName = appointment.User.FullName
                    },
                    StaffNurse = new StaffNurseInfo
                    {
                        StaffNurseId = appointment.StaffNurseId,
                        FullName = await context.Users
                            .Where(u => u.UserId == appointment.StaffNurseId)
                            .Select(u => u.FullName)
                            .FirstOrDefaultAsync()
                    },
                    Topic = appointment.Topic,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentStartTime = appointment.AppointmentStartTime,
                    AppointmentEndTime = appointment.AppointmentEndTime,
                    AppointmentReason = appointment.AppointmentReason,
                    ConfirmationStatus = appointment.ConfirmationStatus,
                    CompletionStatus = appointment.CompletionStatus
                });
            }
            if (response.Count == 0)
            {
                return [];
            }
            return response;
        }

        public async Task<bool> RegisterAppointment(AppointmentRequest request)
        {
            if (request == null)
            {
                return false;
            }

            var userId = await context.Students.Select(s => s.UserId).FirstOrDefaultAsync();
            if (userId == null || userId != request.UserId)
            {
                return false;
            }

            try
            {
                var appointment = new Appointment
                {
                    AppointmentId = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    UserId = request.UserId,
                    StaffNurseId = request.StaffNurseId,
                    Topic = request.Topic,
                    AppointmentDate = request.AppointmentDate,
                    AppointmentStartTime = request.AppointmentStartTime,
                    AppointmentEndTime = request.AppointmentEndTime,
                    AppointmentReason = request.AppointmentReason,
                    ConfirmationStatus = false,
                    CompletionStatus = false
                };
                context.Appointments.Add(appointment);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public Task<AppointmentResponse> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<AppointmentResponse>> GetStaffNurseAppointments(Guid staffNurseId, PaginationRequest? paginationRequest)
        {
            throw new NotImplementedException();
        }
        public Task<AppointmentResponse> GetUserAppointment(Guid userId, Guid appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<AppointmentResponse>> GetUserAppointments(Guid userId, PaginationRequest? paginationRequest)
        {
            throw new NotImplementedException();
        }
    }
}
