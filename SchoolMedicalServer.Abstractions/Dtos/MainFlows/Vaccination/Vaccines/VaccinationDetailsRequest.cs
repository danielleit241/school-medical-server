using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Vaccines
{
    public class VaccinationDetailsRequest
    {
        [Required]
        public string? VaccineCode { get; set; }
        [Required]
        public string? VaccineName { get; set; }
        [Required]
        public string? Manufacturer { get; set; }
        [Required]
        public string? VaccineType { get; set; }
        [Required]
        public string? AgeRecommendation { get; set; }
        [Required]
        public string? BatchNumber { get; set; }
        [Required]
        public DateOnly? ExpirationDate { get; set; }
        [Required]
        public string? ContraindicationNotes { get; set; }

        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
