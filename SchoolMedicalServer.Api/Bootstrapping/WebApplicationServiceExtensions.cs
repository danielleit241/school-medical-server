namespace SchoolMedicalServer.Api.Bootstrapping
{
    public static class WebApplicationServiceExtensions
    {
        public static async Task<WebApplication> AddWebApplicationServicesAsync(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseCors("AllowAllClients");
            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SchoolMedicalManagementContext>();
                context.Database.Migrate();
            }
            await SeedDatabaseAsync(app);

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");
            
            // Add health check endpoint
            app.MapHealthChecks("/health");
            
            return app;
        }

        static async Task SeedDatabaseAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
        }

    }
}
