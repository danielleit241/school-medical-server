using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;
using SchoolMedicalServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Api.Helpers.EmailHelper;

namespace SchoolMedicalServer.Api.Boostraping
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
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
            });

            services.AddDbContext<SchoolMedicalManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DBDefault"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllClients", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddScoped<IAuthServices, AuthServices>();
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IEmailHelper, EmailHelper>();

            return services;
        }
    }
}
