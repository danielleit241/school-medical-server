using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration
{
    public class HealthProfileDeclarationRequest
    {
        [Required]
        public HealthProfileDeclarationDtoRequest HealthDeclaration { get; set; } = default!;

        [Required]
        public List<VaccinationDeclarationDtoRequest>? Vaccinations { get; set; } = [];
    }

    public class HealthProfileDeclarationDtoRequest
    {
        [Required(ErrorMessage = "Student ID is required.")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "Declaration date information is required.")]
        public DateOnly? DeclarationDate { get; set; }

        [Required(ErrorMessage = "Chronic diseases information is required.")]
        public string? ChronicDiseases { get; set; }

        [Required(ErrorMessage = "Drug allergies information is required.")]
        public string? DrugAllergies { get; set; }

        [Required(ErrorMessage = "Food allergies information is required.")]
        public string? FoodAllergies { get; set; }

        public string? Notes { get; set; }
    }

    public class VaccinationDeclarationDtoRequest
    {
        public Guid? VaccinationDeclarationId { get; set; }
        [Required(ErrorMessage = "Vaccine name is required.")]
        public string VaccineName { get; set; } = default!;

        [Required(ErrorMessage = "Dose number is required.")]
        public string? DoseNumber { get; set; }

        [Required(ErrorMessage = "Vaccinated date is required.")]
        public DateOnly? VaccinatedDate { get; set; }

        public string? Notes { get; set; }
    }
}
