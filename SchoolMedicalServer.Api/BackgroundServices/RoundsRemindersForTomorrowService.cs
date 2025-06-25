namespace SchoolMedicalServer.Api.BackgroundServices
{
    public enum ReminderType
    {
        Vaccination,
        HealthCheck
    }

    public class RoundsRemindersForTomorrowService : BackgroundService
    {
        private readonly ILogger<RoundsRemindersForTomorrowService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailHelper _emailHelper;

        public RoundsRemindersForTomorrowService(
            ILogger<RoundsRemindersForTomorrowService> logger,
            IServiceScopeFactory scopeFactory,
            IWebHostEnvironment env,
            IEmailHelper emailHelper)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _env = env;
            _emailHelper = emailHelper;
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

                foreach (ReminderType type in Enum.GetValues(typeof(ReminderType)))
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var reminders = await GetReminderDataForTomorrowAsync(type, scope);
                        _logger.LogInformation("Found {Count} parents with students needing {Type} reminders for tomorrow.", reminders.Count, type);
                        await SendRemindersAsync(reminders, type);
                        _logger.LogInformation("Sent {Type} reminders for tomorrow to {Count} parents.", type, reminders.Count);
                    }
                }
            }
        }

        private async Task<List<(User Parent, List<Student> Students)>> GetReminderDataForTomorrowAsync(ReminderType type, IServiceScope scope)
        {
            switch (type)
            {
                case ReminderType.Vaccination:
                    return await GetVaccinationReminderDataForTomorrowAsync(scope);
                case ReminderType.HealthCheck:
                    return await GetHealthCheckReminderDataForTomorrowAsync(scope);
                default:
                    return new List<(User, List<Student>)>();
            }
        }

        private async Task<List<(User Parent, List<Student> Students)>> GetVaccinationReminderDataForTomorrowAsync(IServiceScope scope)
        {
            var scheduleRepo = scope.ServiceProvider.GetRequiredService<IVaccinationScheduleRepository>();
            var studentRepo = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var resultRepo = scope.ServiceProvider.GetRequiredService<IVaccinationResultRepository>();
            var healthProfileRepo = scope.ServiceProvider.GetRequiredService<IHealthProfileRepository>();
            var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            var schedules = await scheduleRepo.GetVaccinationSchedulesAsync();
            if (schedules == null || !schedules.Any())
                return [];

            var roundsTomorrow = schedules
                .SelectMany(s => s.Rounds)
                .Where(r => r.StartTime != null
                    && DateOnly.FromDateTime((DateTime)r.StartTime) == tomorrow
                    && r.Status == false)
                .ToList();
            if (!roundsTomorrow.Any())
                return [];

            var healthProfileIds = await resultRepo.GetHealthProfileIdsByRoundIdsAsync(
                roundsTomorrow.Select(r => r.RoundId).ToList());
            if (!healthProfileIds.Any())
                return [];

            var healthProfiles = await healthProfileRepo.GetByIdsAsync(healthProfileIds);
            if (!healthProfiles.Any())
                return [];

            var studentIds = healthProfiles.Where(hp => hp.StudentId != null)
                .Select(hp => hp.StudentId!.Value)
                .Distinct()
                .ToList();
            if (!studentIds.Any())
                return [];

            var students = await studentRepo.GetByIdsAsync(studentIds);
            if (!students.Any())
                return [];

            var studentsGroupedByParent = students
                .GroupBy(s => s.UserId)
                .ToList();
            if (!studentsGroupedByParent.Any())
                return [];


            var parentIds = studentsGroupedByParent.Select(g => g.Key!.Value).Distinct().ToList();
            if (!parentIds.Any())
                return [];


            var parents = new List<User>();
            foreach (var pid in parentIds)
            {
                var parent = await userRepo.GetByIdAsync(pid);
                if (parent != null && !string.IsNullOrWhiteSpace(parent.EmailAddress))
                    parents.Add(parent);
            }
            if (!parents.Any())
                return [];

            var parentDict = parents
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.EmailAddress))
                .ToDictionary(p => p!.UserId, p => p!);

            var result = new List<(User Parent, List<Student> Students)>();
            foreach (var group in studentsGroupedByParent)
            {
                var parentId = group.Key!.Value;
                if (!parentDict.TryGetValue(parentId, out var parent) || string.IsNullOrWhiteSpace(parent.EmailAddress))
                    continue;

                var studentsOfParent = group.ToList();
                result.Add((parent, studentsOfParent));
            }

            return result;
        }

        private async Task<List<(User Parent, List<Student> Students)>> GetHealthCheckReminderDataForTomorrowAsync(IServiceScope scope)
        {
            var studentRepo = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var healthProfileRepo = scope.ServiceProvider.GetRequiredService<IHealthProfileRepository>();
            var scheduleRepo = scope.ServiceProvider.GetRequiredService<IHealthCheckScheduleRepository>();
            var resultRepo = scope.ServiceProvider.GetRequiredService<IHealthCheckResultRepository>();

            var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            var schedule = await scheduleRepo.GetHealthCheckSchedulesAsync();
            if (schedule == null) return [];

            var roundsTomorrow = schedule.SelectMany(s => s.Rounds)
                .Where(r => r.StartTime != null
                    && DateOnly.FromDateTime((DateTime)r.StartTime) == tomorrow
                    && r.Status == false)
                .ToList();
            if (!roundsTomorrow.Any()) return [];

            var healthProfileIds = await resultRepo.GetHealthProfileIdsByRoundIdsAsync([.. roundsTomorrow.Select(r => r.RoundId)]);
            if (!healthProfileIds.Any()) return [];

            var healthProfiles = await healthProfileRepo.GetByIdsAsync(healthProfileIds);
            if (!healthProfiles.Any()) return [];

            var studentIds = healthProfiles.Where(hp => hp.StudentId != null)
                .Select(hp => hp.StudentId!.Value)
                .Distinct()
                .ToList();
            if (!studentIds.Any()) return [];

            var students = await studentRepo.GetByIdsAsync(studentIds);
            if (!students.Any()) return [];

            var studentsGroupedByParent = students
                .GroupBy(s => s.UserId)
                .ToList();
            if (!studentsGroupedByParent.Any()) return [];

            var parentIds = studentsGroupedByParent.Select(g => g.Key!.Value).Distinct().ToList();
            if (!parentIds.Any()) return [];

            var parents = new List<User>();
            foreach (var pid in parentIds)
            {
                var parent = await userRepo.GetByIdAsync(pid);
                if (parent != null && !string.IsNullOrWhiteSpace(parent.EmailAddress))
                    parents.Add(parent);
            }
            if (!parents.Any()) return [];

            var parentDict = parents
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.EmailAddress))
                .ToDictionary(p => p!.UserId, p => p!);

            var result = new List<(User Parent, List<Student> Students)>();
            foreach (var group in studentsGroupedByParent)
            {
                var parentId = group.Key!.Value;
                if (!parentDict.TryGetValue(parentId, out var parent) || string.IsNullOrWhiteSpace(parent.EmailAddress))
                    continue;

                var studentsOfParent = group.ToList();
                result.Add((parent, studentsOfParent));
            }
            return result;
        }

        private async Task SendRemindersAsync(List<(User Parent, List<Student> Students)> reminders, ReminderType type)
        {
            if (reminders is null)
            {
                _logger.LogWarning("No reminders to send for type {Type}.", type);
                return;
            }
            string subject = type switch
            {
                ReminderType.Vaccination => "Vaccination Reminder for Tomorrow",
                ReminderType.HealthCheck => "Health Check-up Reminder for Tomorrow",
                _ => "Reminder"
            };

            string templateName = type switch
            {
                ReminderType.Vaccination => "vaccination-reminder-template.html",
                ReminderType.HealthCheck => "healthcheck-reminder-template.html",
                _ => "vaccination-reminder-template.html"
            };
            string templatePath = Path.Combine(_env.WebRootPath, "templates", templateName);

            foreach (var (parent, studentsOfParent) in reminders)
            {
                var studentHtmlList = string.Join("<br>", studentsOfParent.Select(s => $"- {s.FullName} (<b>{s.StudentCode}</b>)"));
                string emailTemplate = await File.ReadAllTextAsync(templatePath);
                string htmlBody = emailTemplate
                    .Replace("{ParentName}", parent.FullName ?? "Parent")
                    .Replace("{DateStr}", DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("MM/dd/yyyy"))
                    .Replace("{StudentsList}", studentHtmlList);

                var request = new EmailFrom
                {
                    Subject = subject,
                    To = parent.EmailAddress!,
                    Body = htmlBody
                };

                await _emailHelper.SendEmailAsync(request);
            }
        }
    }
}