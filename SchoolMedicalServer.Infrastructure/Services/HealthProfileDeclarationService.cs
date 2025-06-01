using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthProfileDeclarationService(SchoolMedicalManagementContext context) : IHealthProfileDeclarationService
    {
        public async Task<bool> CreateHealthDeclarationAsync(HealthProfileDeclarationRequest request)
        {

            var healthProfile = await context.HealthProfiles.FirstOrDefaultAsync(f => f.StudentId == request.HealthDeclaration.StudentId);
            if (healthProfile == null)
            {
                return false;
            }

            if (healthProfile.DeclarationDate.HasValue)
            {
                return false;
            }

            healthProfile.DeclarationDate = request.HealthDeclaration.DeclarationDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            healthProfile.ChronicDiseases = request.HealthDeclaration.ChronicDiseases;
            healthProfile.DrugAllergies = request.HealthDeclaration.DrugAllergies ?? "";
            healthProfile.FoodAllergies = request.HealthDeclaration.FoodAllergies ?? "";
            healthProfile.Notes = request.HealthDeclaration.Notes ?? "";

            if (request.Vaccinations != null)
            {
                foreach (var vaccination in request.Vaccinations)
                {
                    var vaccinationDeclaration = new VaccinationDeclaration
                    {
                        HealthProfileId = healthProfile.HealthProfileId,
                        VaccinationDeclarationId = Guid.NewGuid(),
                        VaccineName = vaccination.VaccineName,
                        DoseNumber = vaccination.DoseNumber,
                        VaccinatedDate = vaccination.VaccinatedDate,
                    };
                    context.VaccinationDeclarations.Add(vaccinationDeclaration);
                }
            }
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<HealthProfileDeclarationResponse?> GetHealthDeclarationAsync(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                return null;
            }
            var healthProfileDeclaration = await context.HealthProfiles
                .Where(h => h.StudentId == studentId)
                .Include(h => h.VaccinationDeclarations)
                .FirstOrDefaultAsync();
            if (healthProfileDeclaration == null)
            {
                return null;
            }

            var healthDeclarationDto = new HealthProfileDeclarationDtoResponse
            {
                HealthProfileId = healthProfileDeclaration.HealthProfileId,
                StudentId = healthProfileDeclaration.StudentId,
                DeclarationDate = healthProfileDeclaration.DeclarationDate,
                ChronicDiseases = healthProfileDeclaration.ChronicDiseases,
                DrugAllergies = healthProfileDeclaration.DrugAllergies,
                FoodAllergies = healthProfileDeclaration.FoodAllergies,
                Notes = healthProfileDeclaration.Notes
            };

            var vaccineDeclarations = healthProfileDeclaration.VaccinationDeclarations
                .Select(v => new VaccinationDeclarationDtoResponse
                {
                    VaccineName = v.VaccineName,
                    DoseNumber = v.DoseNumber,
                    VaccinatedDate = v.VaccinatedDate,
                }).ToList() ?? [];

            var response = new HealthProfileDeclarationResponse()
            {
                HealthDeclaration = healthDeclarationDto,
                Vaccinations = vaccineDeclarations,
            };

            if (response.HealthDeclaration == null)
            {
                return null;
            }

            return response;
        }

    }
}
