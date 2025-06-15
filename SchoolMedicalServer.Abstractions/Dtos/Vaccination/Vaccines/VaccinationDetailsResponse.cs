namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines
{
    public class VaccinationDetailsResponse
    {
        public Guid VaccineId { get; set; }
        public string VaccineCode { get; set; } = default!;
        public string? VaccineName { get; set; }
        public string? Manufacturer { get; set; }
        public string? VaccineType { get; set; }
        public string? AgeRecommendation { get; set; }
        public string? BatchNumber { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? ContraindicationNotes { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
}
