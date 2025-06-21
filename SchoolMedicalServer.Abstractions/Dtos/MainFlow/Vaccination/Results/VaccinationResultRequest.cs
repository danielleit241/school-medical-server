namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.Vaccination.Results
{
    public class VaccinationResultRequest
    {
        public Guid VaccinationResultId { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public DateTime? VaccinatedTime { get; set; }
        public bool Vaccinated { get; set; }
        public string? InjectionSite { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
    }
}
