using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Repositories;

namespace SchoolMedicalServer.Api.Bootstrapping
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IHealthProfileRepository, HealthProfileRepository>();
            services.AddScoped<IMedicalInventoryRepository, MedicalInventoryRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IMedicalRegistrationRepository, MedicalRegistrationRepository>();
            services.AddScoped<IMedicalRegistrationDetailsRepository, MedicalRegistrationDetailsRepository>();
            services.AddScoped<IMedicalEventRepository, MedicalEventRepository>();
            services.AddScoped<IMedicalRequestRepository, MedicalRequestRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IVaccinationDetailsRepository, VaccinationDetailsRepository>();
            services.AddScoped<IVaccinationScheduleRepository, VaccinationScheduleRepository>();
            services.AddScoped<IVaccinationRoundRepository, VaccinationRoundRepository>();
            services.AddScoped<IVaccinationResultRepository, VaccinationResultRepository>();
            services.AddScoped<IVaccinationObservationRepository, VaccinationObservationRepository>();
            services.AddScoped<IHealthCheckScheduleRepository, HealthCheckScheduleRepository>();
            services.AddScoped<IHealthCheckResultRepository, HealthCheckResultRepository>();
            services.AddScoped<IHealthCheckRoundRepository, HealthCheckRoundRepository>();
            return services;
        }
    }
}
