namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class VaccinationScheduleRequest
    {
        public VaccinationScheduleRequestDtoRequest VaccinationScheduleRequestDtoRequest { get; set; } = new();
        public VaccineDetailsRequestDtoRequest VaccineDetailsRequestDtoRequest { get; set; } = new();
    }

    public class VaccinationScheduleRequestDtoRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TargetGrade { get; set; }

        public DateOnly? ParentNotificationStartDate { get; set; }
        public DateOnly? ParentNotificationEndDate { get; set; }
    }

    public class VaccineDetailsRequestDtoRequest
    {
        public string? VaccineCode { get; set; }
        public string? VaccineName { get; set; }
        public string? Manufacturer { get; set; }
        public string? VaccineType { get; set; }
        public string? AgeRecommendation { get; set; }
        public string? BatchNumber { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? ContraindicationNotes { get; set; }
    }
}
