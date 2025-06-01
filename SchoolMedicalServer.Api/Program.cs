using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Api.Boostraping;
using SchoolMedicalServer.Infrastructure;
using SchoolMedicalServer.Infrastructure.Services;

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
                context.Database.Migrate();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User
                    {
                        UserId = Guid.NewGuid(),
                        PhoneNumber = "adminsystem",
                        PasswordHash = new PasswordHasher<User>().HashPassword(null!, "adminsystem"),
                        RoleId = 1
                    });
                    context.SaveChanges();
                }
            }

            app.MapControllers();

            app.Run();
        }
    }
}
