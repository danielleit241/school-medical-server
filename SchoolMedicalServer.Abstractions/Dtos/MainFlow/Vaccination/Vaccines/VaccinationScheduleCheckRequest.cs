using SchoolMedicalServer.Abstractions.Dtos.MainFlow.Vaccination.Schedules;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.Vaccination.Vaccines
{
    public class VaccinationScheduleCheckRequest
    {
        public Guid? VaccineId { get; set; }
        public IEnumerable<VaccinationRoundRequestDto> VaccinationRounds { get; set; } = default!;
    }
}
