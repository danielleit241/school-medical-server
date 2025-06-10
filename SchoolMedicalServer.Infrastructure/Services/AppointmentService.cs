using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AppointmentService(
        IBaseRepository baseRepository,
        IUserRepository userRepository,
        IAppointmentRepository appointmentRepository) : IAppointmentService
    {
        public async Task<IEnumerable<StaffNurseInfo>> GetStaffNurses()
        {
            var staffNurses = await userRepository.GetUsersByRoleName("nurse") as List<User>;

            if (staffNurses == null || staffNurses.Count == 0)
            {
                return [];
            }

            var response = new List<StaffNurseInfo>();
            foreach (var staffNurse in staffNurses)
            {
                response.Add(new StaffNurseInfo
                {
                    StaffNurseId = staffNurse.UserId,
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

            var appointments = await appointmentRepository.GetByStaffNurseAndDateAsync(staffNurseId, date);
            if (appointments == null || appointments.Count == 0)
                return [];

            var user = await userRepository.GetByIdAsync(staffNurseId);

            var nurseFullName = user!.FullName;

            var response = new List<AppointmentResponse>();
            foreach (var appointment in appointments)
            {
                response.Add(await GetResponseAsync(appointment));
            }

            return response;
        }

        public async Task<NotificationRequest> RegisterAppointment(AppointmentRequest request)
        {
            if (request == null)
            {
                return null!;
            }
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return null!;
            }
            var userId = user.UserId;

            var hasAppointment = await appointmentRepository.StaffHasAppointmentAsync(
               request.StaffNurseId, request.AppointmentDate, request.AppointmentStartTime, request.AppointmentEndTime);

            if (hasAppointment)
                return null!;

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
            await appointmentRepository.AddAsync(appointment);
            await baseRepository.SaveChangesAsync();

            return new NotificationRequest
            {
                NotificationTypeId = appointment.AppointmentId,
                SenderId = appointment.UserId,
                ReceiverId = appointment.StaffNurseId
            };
        }

        public async Task<AppointmentResponse> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId)
        {
            var appointment = await appointmentRepository.GetByStaffNurseAndAppointmentIdAsync(staffNurseId, appointmentId);

            if (appointment == null || appointment.User == null || appointment.Student == null)
                return null!;

            var response = await GetResponseAsync(appointment);

            return response;
        }

        public async Task<PaginationResponse<AppointmentResponse>> GetStaffNurseAppointments(Guid staffNurseId, PaginationRequest? paginationRequest)
        {
            var totalCount = await appointmentRepository.CountByStaffNurseIdAsync(staffNurseId);
            if (totalCount == 0)
            {
                return null!;
            }

            int pageIndex = paginationRequest!.PageIndex;
            int pageSize = paginationRequest!.PageSize;
            int skip = (pageIndex - 1) * pageSize;

            var appointments = await appointmentRepository.GetByStaffNursePagedAsync(
                staffNurseId,
                skip,
                pageSize
            );

            if (appointments == null || appointments.Count == 0) return null!;

            var response = new List<AppointmentResponse>();
            foreach (var appointment in appointments)
            {
                response.Add(await GetResponseAsync(appointment));
            }

            return new PaginationResponse<AppointmentResponse>(
                pageIndex,
                pageSize,
                totalCount,
                response
            );
        }

        public async Task<AppointmentResponse> GetUserAppointment(Guid userId, Guid appointmentId)
        {
            var appointment = await appointmentRepository.GetByUserAndAppointmentIdAsync(userId, appointmentId);

            if (appointment == null) return null!;

            var response = await GetResponseAsync(appointment);

            return response;
        }

        public async Task<PaginationResponse<AppointmentResponse>> GetUserAppointments(Guid userId, PaginationRequest? paginationRequest)
        {
            var totalCount = await appointmentRepository.CountByUserIdAsync(userId);
            if (totalCount == 0)
            {
                return null!;
            }

            int pageIndex = paginationRequest!.PageIndex;
            int pageSize = paginationRequest!.PageSize;
            int skip = (pageIndex - 1) * pageSize;

            var appointments = await appointmentRepository.GetByUserPagedAsync(
                userId,
                skip,
                pageSize
            );

            if (appointments == null) return null!;


            var response = new List<AppointmentResponse>();
            foreach (var appointment in appointments)
            {

                response.Add(await GetResponseAsync(appointment));
            }
            return new PaginationResponse<AppointmentResponse>(
                pageIndex,
                pageSize,
                totalCount,
                response
            );
        }


        private async Task<AppointmentResponse> GetResponseAsync(Appointment appointment)
        {
            var user = await userRepository.GetByIdAsync(appointment.UserId);
            if (user == null || appointment.Student == null)
            {
                return null!;
            }
            var nurseFullName = user.FullName;
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

        public async Task<NotificationRequest> ApproveAppointment(Guid appointmentId, AppoinmentNurseApprovedRequest request)
        {
            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null || appointment.StaffNurseId != request.StaffNurseId)
            {
                return null!;
            }
            if (appointment == null)
            {
                return null!;
            }
            if (request.ConfirmationStatus.HasValue)
            {
                appointment.ConfirmationStatus = request.ConfirmationStatus.Value;
            }
            if (request.CompletionStatus.HasValue && appointment.ConfirmationStatus == true)
            {
                appointment.CompletionStatus = request.CompletionStatus.Value;
            }

            appointmentRepository.Update(appointment);
            await baseRepository.SaveChangesAsync();

            return new NotificationRequest
            {
                NotificationTypeId = appointment.AppointmentId,
                SenderId = appointment.StaffNurseId,
                ReceiverId = appointment.UserId
            };
        }
    }
}
