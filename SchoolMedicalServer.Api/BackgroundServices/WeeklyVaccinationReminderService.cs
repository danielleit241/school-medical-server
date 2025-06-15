using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers;
using SchoolMedicalServer.Infrastructure;

namespace SchoolMedicalServer.Api.BackgroundServices
{
    public class WeeklyVaccinationReminderService : BackgroundService
    {
        private readonly ILogger<WeeklyVaccinationReminderService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public WeeklyVaccinationReminderService(
            ILogger<WeeklyVaccinationReminderService> logger,
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

            // 1. Lấy tất cả lịch tiêm chủng
            var schedules = await scheduleRepo.GetVaccinationSchedulesAsync();

            // 2. Lấy tất cả rounds ngày mai
            var roundsTomorrow = schedules
                .SelectMany(s => s.Rounds)
                .Where(r => r.StartTime != null
                    && DateOnly.FromDateTime((DateTime)r.StartTime) == tomorrow
                    && r.Status == false)
                .ToList();

            var healthProfileIds = await resultRepo.GetHealthProfileIdsByRoundIdsAsync(
                roundsTomorrow.Select(r => r.RoundId).ToList());

            // Lấy HealthProfile
            var healthProfiles = await healthProfileRepo.GetByIdsAsync(healthProfileIds);

            // Lấy studentIds từ HealthProfile
            var studentIds = healthProfiles
                .Where(hp => hp.Student!.StudentId! != null)
                .Select(hp => hp.Student.StudentId)
                .Distinct()
                .ToList();

            // Lấy danh sách Student
            //var students = await studentRepo.GetStudentsByIdsAsync(studentIds);


            //// 4. Lấy thông tin học sinh từ repo
            //var allStudents = await studentRepo.GetAllAsync();
            //var targetStudents = allStudents
            //    .Where(s => studentIds.Contains(s.StudentId) && s.UserId != null)
            //    .ToList();

            //// 5. Group theo parent (UserId)
            //var studentsGroupedByParent = targetStudents
            //    .GroupBy(s => s.UserId)
            //    .ToList();

            //// 6. Lấy danh sách parentId
            //var parentIds = studentsGroupedByParent.Select(g => g.Key!.Value).Distinct().ToList();

            //// 7. Lấy thông tin phụ huynh từ repo
            //var parentTasks = parentIds.Select(pid => userRepo.GetByIdAsync(pid));
            //var parents = await Task.WhenAll(parentTasks);
            //var parentDict = parents
            //    .Where(p => p != null && !string.IsNullOrWhiteSpace(p.EmailAddress))
            //    .ToDictionary(p => p!.UserId, p => p!);

            //// 8. Gửi mail cho từng parent
            //foreach (var group in studentsGroupedByParent)
            //{
            //    var parentId = group.Key!.Value;
            //    if (!parentDict.ContainsKey(parentId))
            //        continue; // Bỏ qua nếu parent không có email

            //    var parent = parentDict[parentId];
            //    var studentsOfParent = group.ToList();

            //    string subject = "Nhắc lịch tiêm chủng ngày mai";
            //    string studentList = string.Join("\n", studentsOfParent.Select(s => $"- {s.FullName}"));

            //    string body = $"""
            //            Kính gửi phụ huynh {parent.FullName},

            //            Con của bạn có lịch tiêm chủng vào ngày mai ({tomorrow:dd/MM/yyyy}).

            //            Danh sách học sinh:
            //            {studentList}

            //            Vui lòng kiểm tra hệ thống để biết chi tiết và đảm bảo con em bạn đến đúng giờ.

            //            Trân trọng,
            //            Trường học
            //        """;

            //    var request = new EmailFrom
            //    {
            //        Subject = subject,
            //        To = parent.EmailAddress,
            //        Body = body
            //    };
            //    await emailHelper.SendEmailAsync(request);
            //}
        }
    }
}