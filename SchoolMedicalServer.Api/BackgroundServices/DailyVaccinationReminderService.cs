using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Api.Helpers;

namespace SchoolMedicalServer.Api.BackgroundServices
{
    public class DailyVaccinationReminderService : BackgroundService
    {
        private readonly ILogger<DailyVaccinationReminderService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DailyVaccinationReminderService(
            ILogger<DailyVaccinationReminderService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                _logger.LogCritical($"Vaccination Reminder Service running at {now}.");
                await SendVaccinationRemindersForTomorrow(stoppingToken);
                _logger.LogInformation("Waiting for the next execution at {NextExecutionTime}.",
                   DateTime.Now.AddHours(24));
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
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
                    continue; // Bỏ qua nếu parent không có email

                var studentsOfParent = group.ToList();

                string subject = "Nhắc lịch tiêm chủng ngày mai";
                string studentList = string.Join("\n", studentsOfParent.Select(s => $"- {s.FullName}"));

                string body = $"""
                        Kính gửi phụ huynh {parent.FullName},

                        Con của bạn có lịch tiêm chủng vào ngày mai ({tomorrow:dd/MM/yyyy}).

                        Danh sách học sinh:
                        {studentList}

                        Vui lòng kiểm tra hệ thống để biết chi tiết và đảm bảo con em bạn đến đúng giờ.

                        Trân trọng,
                        Trường học
                    """;

                var request = new EmailFrom
                {
                    Subject = subject,
                    To = parent.EmailAddress!,
                    Body = body
                };
                await emailHelper.SendEmailAsync(request);
            }
        }
    }
}