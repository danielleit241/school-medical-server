namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results
{
    public class VaccinationResultParentResponse
    {
        public VaccineDoseSummary? VaccineDoseSummary { get; set; }
    }
    public class VaccineDoseSummary
    {
        public string? VaccineName { get; set; }
        public string? TotalDoseByVaccineName { get; set; }
        public List<VaccineResultDetailResponse>? VaccineResultDetails { get; set; } = [];
    }

    public class VaccineResultDetailResponse
    {
        public Guid VaccinationResultId { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public string? InjectionSite { get; set; }
        public string? Manufacturer { get; set; }
        public string? BatchNumber { get; set; }
        public int DoseNumber { get; set; }
    }
}
