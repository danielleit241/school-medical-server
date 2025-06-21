using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.MedicalInventory;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class ManagerDashboardService(
        IStudentRepository studentRepository,
        IHealthProfileRepository healthRepository,
        IMedicalRequestRepository medicalRequestRepository,
        IVaccinationResultRepository vaccinationResultRepository,
        IHealthCheckResultRepository healthCheckResultRepository,
        IMedicalInventoryRepository medicalInventoryRepository
        ) : IManagerDashboardService
    {
        public async Task<IEnumerable<IDictionary<string, MedicalInventoryDashboardResponse>>> GetExpiringMedicalItemsAsync()
        {
            var medicalItems = await medicalInventoryRepository.GetAllAsync();
            var today = DateTime.UtcNow.Date;

            var daysUntilSunday = DayOfWeek.Sunday - today.DayOfWeek;
            if (daysUntilSunday < 0) daysUntilSunday += 7;
            var endOfWeek = today.AddDays(daysUntilSunday);

            var expiringThisWeek = medicalItems
                .Where(item => item.ExpiryDate.HasValue && item.ExpiryDate.Value >= today && item.ExpiryDate.Value <= endOfWeek)
                .Select(item => new Dictionary<string, MedicalInventoryDashboardResponse>
                {
                    {
                        item.ItemName!,
                        new MedicalInventoryDashboardResponse
                        {
                            Quantity = item.QuantityInStock,
                            DaysLeft = (item.ExpiryDate!.Value.Date - today).Days,
                            ExpiredDate = item.ExpiryDate.HasValue ? DateOnly.FromDateTime(item.ExpiryDate.Value) : null
                        }
                    }
                })
                .ToList();

            return expiringThisWeek;
        }

        public async Task<IEnumerable<IDictionary<string, int>>> GetLowStockMedicalItemsAsync()
        {
            int count = 10;
            var medicalItems = await medicalInventoryRepository.GetAllAsync();
            var nearestLowStockItems = medicalItems
                .Select(item => new
                {
                    item.ItemName,
                    item.QuantityInStock,
                    item.MinimumStockLevel,
                    Distance = item.QuantityInStock - item.MinimumStockLevel
                })
                .OrderBy(x => x.Distance)
                .ThenBy(x => x.QuantityInStock)
                .Take(count)
                .Select(x => new Dictionary<string, int>
                {
                        { x.ItemName!, x.QuantityInStock }
                })
                .ToList();

            return nearestLowStockItems!;
        }

        public async Task<IEnumerable<DashboardResponse>> GetTotalHealthChecksAsync(DashboardRequest request)
        {
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));
            var responses = new List<DashboardResponse>();

            var results = await healthCheckResultRepository.GetAllAsync();
            results = [.. results.Where(v => v.CreatedAt >= fromDate && v.CreatedAt <= toDate)];

            var completedResults = results.Where(v => v.Status!.ToLower().Contains("completed")).Count();

            var pendingResults = results.Where(v => v.Status!.ToLower().Contains("pending")).Count();

            var failedResults = results.Where(v => v.Status!.ToLower().Contains("failed")).Count();

            var declinedResults = results.Where(v => v.Status!.ToLower().Contains("declined")).Count();

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = completedResults
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = pendingResults
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Failed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = failedResults
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Declined in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = declinedResults
                }
            });
            return responses;
        }

        public async Task<IEnumerable<DashboardResponse>> GetTotalHealthDeclarationsAsync(DashboardRequest request)
        {
            var responses = new List<DashboardResponse>();
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var healthDeclarations = await healthRepository.GetAllAsync();

            var areSubmited = healthDeclarations
                .Where(h => h.DeclarationDate.HasValue &&
                            h.DeclarationDate >= fromDate &&
                            h.DeclarationDate <= toDate)
                .Count();

            var areNotSubmitted = healthDeclarations
                .Where(h => !h.DeclarationDate.HasValue)
                .Count();

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Total Health Declarations in {fromDate} to {toDate}",
                    Count = areSubmited + areNotSubmitted
                }
            });

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Submitted in {fromDate} to {toDate}",
                    Count = areSubmited
                }
            });

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Not Submitted in {fromDate} to {toDate}",
                    Count = areNotSubmitted
                }
            });
            return responses;
        }

        public async Task<IEnumerable<DashboardResponse>> GetTotalMedicalRequestsAsync(DashboardRequest request)
        {
            DateOnly? fromDate = request.From;
            DateOnly? toDate = request.To;

            var medicalRequests = await medicalRequestRepository.GetAllAsync();
            medicalRequests = [.. medicalRequests.Where(mr => mr.RequestDate >= fromDate && mr.RequestDate <= toDate)];

            var totalMedicalRequests = medicalRequests.ToList().Count;
            return
            [
                new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Total Medical Requests in {fromDate} to {toDate}",
                        Count = totalMedicalRequests
                    }
                }
            ];
        }

        public async Task<IEnumerable<DashboardResponse>> GetTotalStudentsAsync(DashboardRequest request)
        {
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));

            var students = await studentRepository.GetAllAsync();
            students = [.. students.Where(s => s.CreatedAt >= fromDate && s.CreatedAt <= toDate)];
            var totalStudents = students.Count;

            return
            [
                new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Total Students in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                        Count = totalStudents
                    }
                }
            ];
        }

            public async Task<IEnumerable<DashboardResponse>> GetTotalVaccinationsAsync(DashboardRequest request)
            {
                DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
                DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));
                var responses = new List<DashboardResponse>();

                var results = await vaccinationResultRepository.GetAllAsync();
                results = [.. results.Where(v => v.CreatedAt >= fromDate && v.CreatedAt <= toDate)];

                var completedResults = results.Where(v => v.Status!.ToLower().Contains("completed")).Count();

                var pendingResults = results.Where(v => v.Status!.ToLower().Contains("pending")).Count();

                var failedResults = results.Where(v => v.Status!.ToLower().Contains("failed")).Count();

                var declinedResults = results.Where(v => v.Status!.ToLower().Contains("declined")).Count();

                var notHealthQualifiedResults = results.Where(v => v.Status!.ToLower().Contains("not qualified")).Count();

                responses.Add(new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Completed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                        Count = completedResults
                    }
                });
                responses.Add(new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Pending in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                        Count = pendingResults
                    }
                });
                responses.Add(new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Failed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                        Count = failedResults
                    }
                });
                responses.Add(new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Declined in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                        Count = declinedResults
                    }
                }); 
                responses.Add(new DashboardResponse
                {
                    Item = new Item
                    {
                        Name = $"Not Health Qualified in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                        Count = notHealthQualifiedResults
                    }
                });
                return responses;
            }
        }
}
