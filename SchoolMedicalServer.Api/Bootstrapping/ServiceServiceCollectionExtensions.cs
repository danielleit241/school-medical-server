using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Bootstrapping
{
    public static class ServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IHealthProfileDeclarationService, HealthProfileDeclarationService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IParentStudentService, ParentStudentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IMedicalRegistrationService, MedicalRegistrationService>();
            services.AddScoped<IMedicalEventService, MedicalEventService>();
            services.AddScoped<IMedicalInventoryService, MedicalInventoryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVaccinationDetailsService, VaccinationDetailsService>();
            services.AddScoped<IVaccinationScheduleService, VaccinationScheduleService>();
            services.AddScoped<IVaccinationRoundService, VaccinationRoundService>();
            services.AddScoped<IVaccinationResultService, VaccinationResultService>();
            services.AddScoped<IHealthCheckScheduleService, HealthCheckScheduleService>();
            services.AddScoped<IHealthCheckResultService, HealthCheckResultService>();
            services.AddScoped<IHealthCheckRoundService, HealthCheckRoundService>();
            services.AddScoped<IMedicalRequestService, MedicalRequestService>();
            return services;
        }
    }
}
