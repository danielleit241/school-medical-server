using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Api.BackgroundServices;
using SchoolMedicalServer.Api.Bootstrapping;
using SchoolMedicalServer.Api.Hubs;
using SchoolMedicalServer.Infrastructure;

namespace SchoolMedicalServer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAllClients");

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SchoolMedicalManagementContext>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                context.Database.Migrate();

                if (!context.Users.Any())
                {
                    var adminSection = config.GetSection("DefaultAdmin");
                    var phoneNumber = adminSection["PhoneNumber"];
                    var password = adminSection["Password"];

                    context.Users.Add(new User
                    {
                        UserId = Guid.NewGuid(),
                        PhoneNumber = phoneNumber!,
                        PasswordHash = new PasswordHasher<User>().HashPassword(null!, password!),
                        RoleId = 1
                    });
                    context.SaveChanges();
                }
            }

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}
