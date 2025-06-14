namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results
{
    public class VaccinationResultRequest
    {
        public Guid VaccinationResultId { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public DateTime? VaccinatedTime { get; set; }
        public bool Vaccinated { get; set; }
        public string? InjectionSite { get; set; }
        public String? Notes { get; set; }
        public String? Status { get; set; }
    }
}
