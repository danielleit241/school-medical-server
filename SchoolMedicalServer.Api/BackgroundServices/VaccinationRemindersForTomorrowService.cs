using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Api.Helpers;

namespace SchoolMedicalServer.Api.BackgroundServices
{
    public class VaccinationRemindersForTomorrowService : BackgroundService
    {
        private readonly ILogger<VaccinationRemindersForTomorrowService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWebHostEnvironment _env;

        public VaccinationRemindersForTomorrowService(ILogger<VaccinationRemindersForTomorrowService> logger, IServiceScopeFactory scopeFactory, IWebHostEnvironment env)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _env = env;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                _logger.LogInformation("Waiting for the next execution at {NextExecutionTime}.", nextRun);

                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.FromMinutes(1);

                await Task.Delay(delay, stoppingToken);

                _logger.LogCritical($"Vaccination Reminder Service running at {now}.");
                await SendVaccinationRemindersForTomorrow(stoppingToken);
            }
        }

        private async Task SendVaccinationRemindersForTomorrow(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var emailHelper = scope.ServiceProvider.GetRequiredService<IEmailHelper>();
            var scheduleRepo = scope.ServiceProvider.GetRequiredService<IVaccinationScheduleRepository>();
            var studentRepo = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var resultRepo = scope.ServiceProvider.GetRequiredService<IVaccinationResultRepository>();
            var healthProfileRepo = scope.ServiceProvider.GetRequiredService<IHealthProfileRepository>();
            var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var schedules = await scheduleRepo.GetVaccinationSchedulesAsync();

            var roundsTomorrow = schedules
                .SelectMany(s => s.Rounds)
                .Where(r => r.StartTime != null
                    && DateOnly.FromDateTime((DateTime)r.StartTime) == tomorrow
                    && r.Status == false)
                .ToList();

            var healthProfileIds = await resultRepo.GetHealthProfileIdsByRoundIdsAsync(
                roundsTomorrow.Select(r => r.RoundId).ToList());

            var healthProfiles = await healthProfileRepo.GetByIdsAsync(healthProfileIds);

            var studentIds = healthProfiles.Where(hp => hp.StudentId != null)
                .Select(hp => hp.StudentId!.Value)
                .Distinct()
                .ToList();

            var students = await studentRepo.GetByIdsAsync(studentIds);

            var studentsGroupedByParent = students
                .GroupBy(s => s.UserId)
                .ToList();

            var parentIds = studentsGroupedByParent.Select(g => g.Key!.Value).Distinct().ToList();

            var parents = new List<User>();
            foreach (var pid in parentIds)
            {
                var parent = await userRepo.GetByIdAsync(pid);
                if (parent != null && !string.IsNullOrWhiteSpace(parent.EmailAddress))
                    parents.Add(parent);
            }
            var parentDict = parents
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.EmailAddress))
                .ToDictionary(p => p!.UserId, p => p!);

            foreach (var group in studentsGroupedByParent)
            {
                var parentId = group.Key!.Value;
                if (!parentDict.TryGetValue(parentId, out var parent) || string.IsNullOrWhiteSpace(parent.EmailAddress))
                    continue;

                var studentsOfParent = group.ToList();

                string subject = "Vaccination Reminder for Tomorrow";
                string templatePath = Path.Combine(_env.WebRootPath, "templates", "vaccination-reminder-template.html");
                var studentHtmlList = string.Join("<br>", studentsOfParent.Select(s => $"- {s.FullName} (<b>{s.StudentCode}</b>)"));
                string emailTemplate = await File.ReadAllTextAsync(templatePath);
                string htmlBody = emailTemplate
                    .Replace("{ParentName}", parent.FullName ?? "Parent")
                    .Replace("{DateStr}", tomorrow.ToString("MM/dd/yyyy"))
                    .Replace("{StudentsList}", studentHtmlList);

                var request = new EmailFrom
                {
                    Subject = subject,
                    To = parent.EmailAddress!,
                    Body = htmlBody
                };
                await emailHelper.SendEmailAsync(request);
            }
        }
    }
}