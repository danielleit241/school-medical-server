using SchoolMedicalServer.Abstractions.Dtos.Dashboard;
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
            var endOf2Week = today.AddDays(13);

            var expiringThisWeek = medicalItems
                .Where(item => item.ExpiryDate.HasValue && item.ExpiryDate.Value >= today && item.ExpiryDate.Value <= endOf2Week)
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

        public async Task<IEnumerable<IDictionary<string, LowStockDashboardResponse>>> GetLowStockMedicalItemsAsync()
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
                .Select(x => new Dictionary<string, LowStockDashboardResponse>
                {
                        {
                            x.ItemName!,
                            new LowStockDashboardResponse
                            {
                                QuantityInStock = x.QuantityInStock,
                                MinimumStockLevel = x.MinimumStockLevel,
                            }
                        }
                })
                .ToList();

            return nearestLowStockItems!;
        }

        public async Task<IEnumerable<DashboardResponse>> GetTotalHealthCheckResultsAsync(DashboardRequest request)
        {
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));
            var responses = new List<DashboardResponse>();

            var results = await healthCheckResultRepository.GetAllAsync();
            results = [.. results.Where(v => v.CreatedAt >= fromDate && v.CreatedAt <= toDate)];

            var completedResults = results.Where(v => v.Status!.ToLower().Contains("completed")).ToList();

            var pendingResults = results.Where(v => v.Status!.ToLower().Contains("pending")).ToList();

            var failedResults = results.Where(v => v.Status!.ToLower().Contains("failed")).ToList();

            var declinedResults = results.Where(v => v.Status!.ToLower().Contains("declined")).ToList();

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = completedResults.Count,
                    Details = completedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.ResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = pendingResults.Count,
                    Details = pendingResults.Select(detail => new ItemDetails
                    {
                        Id = detail.ResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Failed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = failedResults.Count,
                    Details = failedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.ResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Declined in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = declinedResults.Count,
                    Details = declinedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.ResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
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
                .ToList();

            var areNotSubmitted = healthDeclarations
                .Where(h => !h.DeclarationDate.HasValue)
                .ToList();

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Total Health Declarations in {fromDate} to {toDate}",
                    Count = areSubmited.Count + areNotSubmitted.Count
                }
            });

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Submitted in {fromDate} to {toDate}",
                    Count = areSubmited.Count,
                    Details = areSubmited.Select(detail => new ItemDetails
                    {
                        Id = detail.HealthProfileId
                    }).ToList()
                }
            });

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Not Submitted in {fromDate} to {toDate}",
                    Count = areNotSubmitted.Count,
                    Details = areNotSubmitted.Select(detail => new ItemDetails
                    {
                        Id = detail.HealthProfileId
                    }).ToList()
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
                        Count = totalMedicalRequests,
                        Details = medicalRequests.Select(detail => new ItemDetails{
                            Id = detail.RequestId,
                            Name = detail.Item!.ItemName
                        }).ToList()
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

        public async Task<IEnumerable<DashboardResponse>> GetTotalVaccinationResultsAsync(DashboardRequest request)
        {
            DateTime? fromDate = request.From?.ToDateTime(new TimeOnly(0, 0));
            DateTime? toDate = request.To?.ToDateTime(new TimeOnly(23, 59));
            var responses = new List<DashboardResponse>();

            var results = await vaccinationResultRepository.GetAllAsync();
            results = [.. results.Where(v => v.CreatedAt >= fromDate && v.CreatedAt <= toDate)];

            var completedResults = results.Where(v => v.Status!.ToLower().Contains("completed")).ToList();

            var pendingResults = results.Where(v => v.Status!.ToLower().Contains("pending")).ToList();

            var failedResults = results.Where(v => v.Status!.ToLower().Contains("failed")).ToList();

            var declinedResults = results.Where(v => v.Status!.ToLower().Contains("declined")).ToList();

            var notHealthQualifiedResults = results.Where(v => v.Status!.ToLower().Contains("not qualified")).ToList();

            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Completed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = completedResults.Count,
                    Details = completedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.VaccinationResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Pending in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = pendingResults.Count,
                    Details = pendingResults.Select(detail => new ItemDetails
                    {
                        Id = detail.VaccinationResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Failed in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = failedResults.Count,
                    Details = failedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.VaccinationResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Declined in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = declinedResults.Count,
                    Details = declinedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.VaccinationResultId,
                        Name = detail.HealthProfile!.Student.FullName
                    }).ToList()
                }
            });
            responses.Add(new DashboardResponse
            {
                Item = new Item
                {
                    Name = $"Not Health Qualified in {DateOnly.FromDateTime(fromDate!.Value)} to {DateOnly.FromDateTime(toDate!.Value)}",
                    Count = notHealthQualifiedResults.Count,
                    Details = notHealthQualifiedResults.Select(detail => new ItemDetails
                    {
                        Id = detail.VaccinationResultId
                    }).ToList()
                }
            });
            return responses;
        }
    }
}
