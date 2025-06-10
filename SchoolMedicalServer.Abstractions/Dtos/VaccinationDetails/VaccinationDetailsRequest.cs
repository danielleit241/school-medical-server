namespace SchoolMedicalServer.Abstractions.Dtos.VaccinationDetails
{
    public class VaccinationDetailsRequest
    {
        public string? VaccineCode { get; set; }
        public string? VaccineName { get; set; }
        public string? Manufacturer { get; set; }
        public string? VaccineType { get; set; }
        public string? AgeRecommendation { get; set; }
        public string? BatchNumber { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? ContraindicationNotes { get; set; }
        public string? Description { get; set; }
    }
}
