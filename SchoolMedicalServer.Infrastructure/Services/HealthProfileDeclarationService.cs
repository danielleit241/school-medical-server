using SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthProfileDeclarationService(
        IBaseRepository baseRepository,
        IHealthProfileRepository healthProfileRepository) : IHealthProfileDeclarationService
    {
        public async Task<bool> CreateHealthDeclarationAsync(HealthProfileDeclarationRequest request)
        {

            var healthProfile = await healthProfileRepository.GetByStudentIdAsync(request.HealthDeclaration.StudentId);
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
                    await healthProfileRepository.AddVaccinationDeclarationAsync(vaccinationDeclaration);
                }
            }

            await baseRepository.SaveChangesAsync();
            return true;

        }

        public async Task<HealthProfileDeclarationResponse?> GetHealthDeclarationAsync(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                return null;
            }
            var healthProfileDeclaration = await healthProfileRepository.GetByStudentIdWithVaccinationsAsync(studentId);
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
                Notes = healthProfileDeclaration.Notes,
                IsDeclaration = healthProfileDeclaration.DeclarationDate == null ? false : true,
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

        public async  Task<bool> UpdateHealthDeclarationAsync(HealthProfileDeclarationRequest request)
        {
            var studentId = request.HealthDeclaration.StudentId;
            if (studentId == Guid.Empty)
            {
                return false;
            }

            
            var healthProfile = await healthProfileRepository.GetByStudentIdWithVaccinationsAsync(studentId);
            if (healthProfile == null)
            {
                return false;
            }

          
            healthProfile.DeclarationDate = request.HealthDeclaration.DeclarationDate ?? healthProfile.DeclarationDate;
            healthProfile.ChronicDiseases = request.HealthDeclaration.ChronicDiseases ?? healthProfile.ChronicDiseases;
            healthProfile.DrugAllergies = request.HealthDeclaration.DrugAllergies ?? healthProfile.DrugAllergies;
            healthProfile.FoodAllergies = request.HealthDeclaration.FoodAllergies ?? healthProfile.FoodAllergies;
            healthProfile.Notes = request.HealthDeclaration.Notes ?? healthProfile.Notes;

           
            if (request.Vaccinations != null)
            {
                healthProfile.VaccinationDeclarations.Clear();
                foreach (var vaccination in request.Vaccinations)
                {
                    var vaccinationDeclaration = new VaccinationDeclaration
                    {
                        HealthProfileId = healthProfile.HealthProfileId,
                        VaccinationDeclarationId = Guid.NewGuid(),
                        VaccineName = vaccination.VaccineName,
                        DoseNumber = vaccination.DoseNumber,
                        VaccinatedDate = vaccination.VaccinatedDate
               
                    };
                    healthProfile.VaccinationDeclarations.Add(vaccinationDeclaration);
                }
            }

            await baseRepository.SaveChangesAsync();
            return true;
        }
    }
}
