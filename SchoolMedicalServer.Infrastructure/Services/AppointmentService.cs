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
                    AppointmentId = appointment.AppointmentId,
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

            var userId = await context.Students.Where(u => u.UserId == request.UserId).Select(s => s.UserId).FirstOrDefaultAsync();
            if (userId == null)
            {
                return false;
            }

            var isStaffHasAppointment = await context.Appointments
                .AnyAsync(a => a.StaffNurseId == request.StaffNurseId && a.AppointmentDate == request.AppointmentDate &&
                               a.AppointmentStartTime < request.AppointmentEndTime && a.AppointmentEndTime > request.AppointmentStartTime);
            if (isStaffHasAppointment)
                return false;

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
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<AppointmentResponse> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId)
        {
            var appointment = await context.Appointments
                            .Include(u => u.User)
                            .Include(s => s.Student)
                            .FirstOrDefaultAsync(a => a.StaffNurseId == staffNurseId && a.AppointmentId == appointmentId);

            if (appointment == null || appointment.User == null || appointment.Student == null)
                return null!;

            var nurseFullName = await context.Users
                .Where(u => u.UserId == staffNurseId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync();

            var response = new AppointmentResponse
            {
                Student = new StudentInfo
                {
                    StudentId = appointment.Student.StudentId,
                    StudentCode = appointment.Student.StudentCode,
                    FullName = appointment.Student.FullName
                },
                User = new UserInfo
                {
                    UserId = appointment.User.UserId,
                    FullName = appointment.User.FullName
                },
                StaffNurse = new StaffNurseInfo
                {
                    StaffNurseId = appointment.StaffNurseId,
                    FullName = nurseFullName
                },
                AppointmentId = appointment.AppointmentId,
                Topic = appointment.Topic,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentStartTime = appointment.AppointmentStartTime,
                AppointmentEndTime = appointment.AppointmentEndTime,
                AppointmentReason = appointment.AppointmentReason,
                ConfirmationStatus = appointment.ConfirmationStatus,
                CompletionStatus = appointment.CompletionStatus
            };

            return response;
        }

        public async Task<PaginationResponse<AppointmentResponse>> GetStaffNurseAppointments(Guid staffNurseId, PaginationRequest? paginationRequest)
        {
            var totalCount = await context.Appointments.CountAsync(a => a.StaffNurseId == staffNurseId);
            if (totalCount == 0)
            {
                return null!;
            }

            int pageIndex = paginationRequest!.PageIndex;
            int pageSize = paginationRequest!.PageSize;
            int skip = (pageIndex - 1) * pageSize;

            var appointments = await context.Appointments
                .Include(a => a.User)
                .Include(a => a.Student)
                .Where(a => a.StaffNurseId == staffNurseId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentStartTime)
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            if (appointments == null || appointments.Count == 0) return null!;

            var nurseFullName = await context.Users
                .Where(u => u.UserId == staffNurseId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync() ?? string.Empty;

            var response = appointments.Select(appointment => new AppointmentResponse
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
                    FullName = nurseFullName
                },
                AppointmentId = appointment.AppointmentId,
                Topic = appointment.Topic,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentStartTime = appointment.AppointmentStartTime,
                AppointmentEndTime = appointment.AppointmentEndTime,
                AppointmentReason = appointment.AppointmentReason,
                ConfirmationStatus = appointment.ConfirmationStatus,
                CompletionStatus = appointment.CompletionStatus
            }).ToList();

            return new PaginationResponse<AppointmentResponse>(
                pageIndex,
                pageSize,
                totalCount,
                response
            );
        }

        public async Task<AppointmentResponse> GetUserAppointment(Guid userId, Guid appointmentId)
        {
            var appointment = await context.Appointments
                           .Include(u => u.User)
                           .Include(s => s.Student)
                           .FirstOrDefaultAsync(a => a.UserId == userId && a.AppointmentId == appointmentId);

            if (appointment == null) return null!;

            var nurseFullName = await context.Users
                .Where(u => u.UserId == appointment.StaffNurseId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync();

            var response = new AppointmentResponse
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
                    FullName = nurseFullName
                },
                AppointmentId = appointment.AppointmentId,
                Topic = appointment.Topic,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentStartTime = appointment.AppointmentStartTime,
                AppointmentEndTime = appointment.AppointmentEndTime,
                AppointmentReason = appointment.AppointmentReason,
                ConfirmationStatus = appointment.ConfirmationStatus,
                CompletionStatus = appointment.CompletionStatus
            };

            return response;
        }

        public async Task<PaginationResponse<AppointmentResponse>> GetUserAppointments(Guid userId, PaginationRequest? paginationRequest)
        {
            var totalCount = await context.Appointments.CountAsync(a => a.UserId == userId);
            if (totalCount == 0)
            {
                return null!;
            }

            int pageIndex = paginationRequest!.PageIndex;
            int pageSize = paginationRequest!.PageSize;
            int skip = (pageIndex - 1) * pageSize;

            var appointments = await context.Appointments
                .Include(a => a.User)
                .Include(a => a.Student)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentStartTime)
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            if (appointments == null) return null!;


            var response = new List<AppointmentResponse>();
            foreach (var appointment in appointments)
            {
                var nurseFullName = await context.Users
                    .Where(u => u.UserId == appointment.StaffNurseId)
                    .Select(u => u.FullName)
                    .FirstOrDefaultAsync();
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
                        FullName = nurseFullName
                    },
                    AppointmentId = appointment.AppointmentId,
                    Topic = appointment.Topic,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentStartTime = appointment.AppointmentStartTime,
                    AppointmentEndTime = appointment.AppointmentEndTime,
                    AppointmentReason = appointment.AppointmentReason,
                    ConfirmationStatus = appointment.ConfirmationStatus,
                    CompletionStatus = appointment.CompletionStatus
                });
            }
            return new PaginationResponse<AppointmentResponse>(
                pageIndex,
                pageSize,
                totalCount,
                response
            );
        }

        public async Task<bool> ApproveAppointment(Guid appointmentId, AppoinmentNurseApprovedRequest request)
        {
            var appointment = await context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (appointment == null || appointment.StaffNurseId != request.StaffNurseId)
            {
                return false;
            }
            if (appointment == null)
            {
                return false;
            }
            if (request.ConfirmationStatus.HasValue)
            {
                appointment.ConfirmationStatus = request.ConfirmationStatus.Value;
            }
            if (request.CompletionStatus.HasValue && appointment.ConfirmationStatus == true)
            {
                appointment.CompletionStatus = request.CompletionStatus.Value;
            }
            try
            {
                context.Appointments.Update(appointment);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
