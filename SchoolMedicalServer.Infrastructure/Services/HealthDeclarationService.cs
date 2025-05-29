//using Microsoft.EntityFrameworkCore;
//using SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration;
//using SchoolMedicalServer.Abstractions.Entities;
//using SchoolMedicalServer.Abstractions.IServices;

//namespace SchoolMedicalServer.Infrastructure.Services
//{
//    public class HealthDeclarationService(SchoolMedicalManagementContext context) : IHealthDeclarationService
//    {
//        public async Task<bool> CreateHealthDeclarationAsync(Guid studentId, HealthDeclarationRequest request)
//        {
//            if (studentId == Guid.Empty)
//            {
//                return false;
//            }

//            if (request == null)
//            {
//                return false;
//            }

//            var student = await context.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
//            if (student == null)
//            {
//                return false;
//            }

//            var healthDeclarationId = Guid.NewGuid();
//            var healthDeclaration = new HealthDeclaration
//            {
//                HealthDeclarationId = healthDeclarationId,
//                //StudentId = studentId,
//                DeclarationDate = request.HealthDeclaration.DeclarationDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
//                ChronicDiseases = request.HealthDeclaration.ChronicDiseases,
//                DrugAllergies = request.HealthDeclaration.DrugAllergies ?? "",
//                FoodAllergies = request.HealthDeclaration.FoodAllergies ?? "",
//                Notes = request.HealthDeclaration.Notes ?? ""
//            };
//            context.HealthDeclarations.Add(healthDeclaration);
//            if (request.Vaccinations != null)
//            {
//                foreach (var vaccination in request.Vaccinations)
//                {
//                    var vaccinationDeclaration = new VaccinationDeclaration
//                    {
//                        HealthDeclarationId = healthDeclarationId,
//                        VaccinationDeclarationId = Guid.NewGuid(),
//                        VaccineName = vaccination.VaccineName,
//                        BatchNumber = vaccination.BatchNumber,
//                        VaccinatedDate = vaccination.VaccinatedDate,
//                        Notes = vaccination.Notes
//                    };
//                    context.VaccinationDeclarations.Add(vaccinationDeclaration);
//                }
//            }
//            try
//            {
//                await context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        public async Task<HealthDeclarationResponse?> GetHealthDeclarationAsync(Guid studentId)
//        {
//            if (studentId == Guid.Empty)
//            {
//                return null;
//            }
//            var healthDeclaration = await context.HealthDeclarations
//                .Include(h => h.VaccinationDeclarations)
//                .FirstOrDefaultAsync();
//            if (healthDeclaration == null)
//            {
//                return null;
//            }

//            var healthDeclarationDto = new HealthDeclarationDtoResponse
//            {
//                HealthDeclarationId = healthDeclaration.HealthDeclarationId,
//                //StudentId = healthDeclaration.StudentId,
//                DeclarationDate = healthDeclaration.DeclarationDate,
//                ChronicDiseases = healthDeclaration.ChronicDiseases,
//                DrugAllergies = healthDeclaration.DrugAllergies,
//                FoodAllergies = healthDeclaration.FoodAllergies,
//                Notes = healthDeclaration.Notes
//            };

//            var vaccineDeclarations = healthDeclaration.VaccinationDeclarations
//                .Select(v => new VaccinationDeclarationDtoResponse
//                {
//                    VaccineName = v.VaccineName,
//                    BatchNumber = v.BatchNumber,
//                    VaccinatedDate = v.VaccinatedDate,
//                    Notes = v.Notes
//                }).ToList() ?? [];

//            var response = new HealthDeclarationResponse()
//            {
//                HealthDeclaration = healthDeclarationDto,
//                Vaccinations = vaccineDeclarations,
//            };

//            if (response.HealthDeclaration == null)
//            {
//                return null;
//            }

//            return response;
//        }

//    }
//}
