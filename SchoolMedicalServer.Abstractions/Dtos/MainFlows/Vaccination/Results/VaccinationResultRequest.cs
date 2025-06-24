using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Results
{
    public class VaccinationResultRequest
    {
        [Required]
        public Guid VaccinationResultId { get; set; }
        [Required]
        public DateOnly? VaccinatedDate { get; set; }
        [Required]
        public DateTime? VaccinatedTime { get; set; }
        [Required]
        public bool Vaccinated { get; set; }
        [Required]
        public string? InjectionSite { get; set; }

        public string? Notes { get; set; }
        [Required]
        public string? Status { get; set; }
    }
}
