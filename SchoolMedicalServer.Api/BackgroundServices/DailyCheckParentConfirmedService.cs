
using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Api.Helpers;

namespace SchoolMedicalServer.Api.BackgroundServices
{
    public class DailyCheckParentConfirmedService : BackgroundService
    {
        private readonly ILogger<DailyCheckParentConfirmedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailHelper _email;

        public DailyCheckParentConfirmedService(ILogger<DailyCheckParentConfirmedService> logger, IServiceScopeFactory scopeFactory, IWebHostEnvironment env, IEmailHelper email)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _env = env;
            _email = email;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sendMailTask = RunDailyJobAtHourAsync(7, SendParentReminderEmailsAsync, stoppingToken);
            var ConfirmTask = RunDailyJobAtHourAsync(24, CloseParentConfirmationAsync, stoppingToken);

            //var sendMailTask = TestRunDailyJobAtHourAsync(7, SendParentReminderEmailsAsync, stoppingToken);
            //var ConfirmTask = TestRunDailyJobAtHourAsync(0, CloseParentConfirmationAsync, stoppingToken);

            await Task.WhenAll(sendMailTask, ConfirmTask);
        }

        //private async Task TestRunDailyJobAtHourAsync(int hour, Func<CancellationToken, IServiceScope, Task> job, CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        var delay = TimeSpan.FromSeconds(10);
        //        _logger.LogInformation("Test mode: Waiting {Delay} before running job at {Hour}h", delay, hour);
        //        await Task.Delay(delay, stoppingToken);

        //        using var scope = _scopeFactory.CreateScope();
        //        try
        //        {
        //            await job(stoppingToken, scope);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Error when running job at {hour}h");
        //        }
        //    }
        //}

        private async Task RunDailyJobAtHourAsync(int hour, Func<CancellationToken, IServiceScope, Task> job, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                DateTime nextRun = now.Date.AddHours(hour).AddMinutes(-1);
                _logger.LogInformation("Next run for job at {Hour}h is scheduled for {NextRun}", hour, nextRun);
                if (now > nextRun)
                    nextRun = nextRun.AddDays(1);

                var delay = nextRun - now;
                if (delay > TimeSpan.Zero)
                {
                    _logger.LogInformation("Waiting {Delay} before running job at {Hour}h", delay, hour);
                    await Task.Delay(delay, stoppingToken);
                }

                using var scope = _scopeFactory.CreateScope();
                try
                {
                    await job(stoppingToken, scope);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error when running job at {hour}h");
                }
            }
        }
        private async Task SendParentReminderEmailsAsync(CancellationToken stoppingToken, IServiceScope scope)
        {
            var today = DateTime.Today;
            int diffToMonday = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = today.AddDays(-diffToMonday);
            var sunday = monday.AddDays(6);

            var healthCheckScheduleRepo = scope.ServiceProvider.GetRequiredService<IHealthCheckScheduleRepository>();
            var healthCheckresultRepo = scope.ServiceProvider.GetRequiredService<IHealthCheckResultRepository>();
            var vaccinationScheduleRepo = scope.ServiceProvider.GetRequiredService<IVaccinationScheduleRepository>();
            var vaccinationResultRepo = scope.ServiceProvider.GetRequiredService<IVaccinationResultRepository>();

            var healthCheckSchedules = await healthCheckScheduleRepo.GetHealthCheckSchedulesByDateRange(monday, sunday);
            var vaccinationSchedules = await vaccinationScheduleRepo.GetVaccinationSchedulesByDateRange(monday, sunday);

            var healthCheckResultsReminders = healthCheckSchedules?
                .Where(s => s.ParentNotificationEndDate == DateOnly.FromDateTime(today.AddDays(1)))
                .SelectMany(s => s.Rounds)
                .SelectMany(r => r.HealthCheckResults)
                .Where(r => r.ParentConfirmed == null)
                .ToList();

            var vaccinationResultsReminders = vaccinationSchedules?
                .Where(s => s.ParentNotificationEndDate == DateOnly.FromDateTime(today.AddDays(1)))
                .SelectMany(s => s.Rounds)
                .SelectMany(r => r.VaccinationResults)
                .Where(r => r.ParentConfirmed == null)
                .ToList();

            _logger.LogInformation("Reminder emails ready to be sent for {HealthCount} health check and {VaccCount} vaccination results.",
                healthCheckResultsReminders?.Count ?? 0, vaccinationResultsReminders?.Count ?? 0);

            string templateName = "email_schedule_reminders.html";
            string templatePath = Path.Combine(_env.WebRootPath, "templates", templateName);
            if (!File.Exists(templatePath))
            {
                _logger.LogError("Template file {TemplateName} not found at {TemplatePath}.", templateName, templatePath);
                return;
            }
            foreach (var result in healthCheckResultsReminders ?? [])
            {
                var rs = await healthCheckresultRepo.GetByIdAsync(result.ResultId);
                var parent = rs!.HealthProfile!.Student.User;

                _logger.LogInformation("Sending reminder email for health check result {Id}.", result.ResultId);
                string emailTemplate = await File.ReadAllTextAsync(templatePath);
                string html = emailTemplate
                    .Replace("{ParentName}", parent!.FullName ?? "Parent")
                    .Replace("{Type}", "Health Check");
                await _email.SendEmailAsync(new EmailFrom
                {
                    To = parent.EmailAddress!,
                    Subject = "[MEDICARE] Please confirm Health Check result",
                    Body = html
                });
            }

            foreach (var result in vaccinationResultsReminders ?? [])
            {
                var rs = await vaccinationResultRepo.GetByIdAsync(result.VaccinationResultId);
                var parent = rs!.HealthProfile!.Student.User;
                _logger.LogInformation("Sending reminder email for vaccination result {Id}.", result.VaccinationResultId);
                string emailTemplate = await File.ReadAllTextAsync(templatePath);
                string html = emailTemplate
                    .Replace("{ParentName}", parent!.FullName ?? "Parent")
                    .Replace("{Type}", "Vaccination");
                await _email.SendEmailAsync(new EmailFrom
                {
                    To = parent.EmailAddress!,
                    Subject = "[MEDICARE] Please confirm Vaccination result",
                    Body = html
                });
            }
        }

        private async Task CloseParentConfirmationAsync(CancellationToken stoppingToken, IServiceScope scope)
        {
            var today = DateTime.Today;
            int diffToMonday = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = today.AddDays(-diffToMonday);
            var sunday = monday.AddDays(6);

            var healthCheckScheduleRepo = scope.ServiceProvider.GetRequiredService<IHealthCheckScheduleRepository>();
            var vaccinationScheduleRepo = scope.ServiceProvider.GetRequiredService<IVaccinationScheduleRepository>();
            var healthCheckResultRepo = scope.ServiceProvider.GetRequiredService<IHealthCheckResultRepository>();
            var vaccinationResultRepo = scope.ServiceProvider.GetRequiredService<IVaccinationResultRepository>();

            var healthCheckSchedules = await healthCheckScheduleRepo.GetHealthCheckSchedulesByDateRange(monday, sunday);
            var vaccinationSchedules = await vaccinationScheduleRepo.GetVaccinationSchedulesByDateRange(monday, sunday);

            var healthCheckResults = healthCheckSchedules?
                .Where(s => s.ParentNotificationEndDate == DateOnly.FromDateTime(today))
                .SelectMany(s => s.Rounds)
                .SelectMany(r => r.HealthCheckResults)
                .Where(r => r.ParentConfirmed == null)
                .ToList();
            var vaccinationResults = vaccinationSchedules?
                .Where(s => s.ParentNotificationEndDate == DateOnly.FromDateTime(today))
                .SelectMany(s => s.Rounds)
                .SelectMany(r => r.VaccinationResults)
                .Where(r => r.ParentConfirmed == null)
                .ToList();
            foreach (var healthCheckResult in healthCheckResults ?? [])
            {
                if (healthCheckResult.ParentConfirmed is null)
                {
                    healthCheckResult.ParentConfirmed = false;
                    healthCheckResult.Status = "Failed";
                    healthCheckResult.Notes = "Parent did not confirm health check result.";
                    _logger.LogInformation("Closing parent confirmation for health check result {Id}.", healthCheckResult.ResultId);
                    await healthCheckResultRepo.UpdateAsync(healthCheckResult);
                }
            }
            foreach (var vaccinationResult in vaccinationResults ?? [])
            {
                if (vaccinationResult.ParentConfirmed is null)
                {
                    vaccinationResult.ParentConfirmed = false;
                    vaccinationResult.Status = "Failed";
                    vaccinationResult.Notes = "Parent did not confirm vaccination result.";
                    _logger.LogInformation("Closing parent confirmation for vaccination result {Id}.", vaccinationResult.VaccinationResultId);
                    await vaccinationResultRepo.UpdateAsync(vaccinationResult);
                }
            }
        }
    }
}
