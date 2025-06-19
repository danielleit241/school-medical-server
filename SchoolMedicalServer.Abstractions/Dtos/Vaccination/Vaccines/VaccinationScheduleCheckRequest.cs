using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;

namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines
{
    public class VaccinationScheduleCheckRequest
    {
        public Guid? VaccineId { get; set; }
        public IEnumerable<VaccinationRoundRequestDto> VaccinationRounds { get; set; } = default!;
    }
}
