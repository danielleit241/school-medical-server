using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Bootstrapping
{
    public static class ServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddScoped<IHealthProfileDeclarationService, HealthProfileDeclarationService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IParentStudentService, ParentStudentService>();
            services.AddTransient<IFileService, FileService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddTransient<IMedicalRegistrationService, MedicalRegistrationService>();
            services.AddTransient<IMedicalEventService, MedicalEventService>();
            services.AddScoped<IMedicalInventoryService, MedicalInventoryService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVaccinationDetailsService, VaccinationDetailsService>();
            services.AddScoped<IVaccinationScheduleService, VaccinationScheduleService>();
            services.AddScoped<IVaccinationRoundService, VaccinationRoundService>();
            services.AddScoped<IVaccinationResultService, VaccinationResultService>();
            return services;
        }
    }
}
