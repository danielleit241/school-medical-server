using System.ComponentModel.DataAnnotations;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Schedules;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Vaccines
{
    public class VaccinationCheckRequest
    {
        [Required]
        public Guid? VaccineId { get; set; }
        [Required]
        public IEnumerable<VaccinationRoundRequestDto> VaccinationRounds { get; set; } = default!;
    }
}
