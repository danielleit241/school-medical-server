using Microsoft.OpenApi.Models;
using SchoolMedicalServer.Api.Boostraping;
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

            app.MapControllers();

            app.Run();
        }
    }
}
