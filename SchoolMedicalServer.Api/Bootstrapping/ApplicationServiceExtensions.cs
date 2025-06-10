using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;
using SchoolMedicalServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SchoolMedicalServer.Api.Helpers;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Repositories;

namespace SchoolMedicalServer.Api.Boostraping
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "School Medical API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                         {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                         },
                         Array.Empty<string>()
                     }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddDbContext<SchoolMedicalManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DBDefault"),
                    sqlOptions => sqlOptions.MigrationsAssembly("SchoolMedicalServer.Infrastructure"));
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllClients", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

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
            services.AddScoped<IVacctionDetailsRepository, VaccinationDetailsRepository>();

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

            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.AddTransient<IEmailHelper, EmailHelper>();

            return services;
        }
    }
}
