namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules
{
    public class VaccinationScheduleResponse
    {
        public VaccinationScheduleResponseDto VaccinationScheduleResponseDto { get; set; } = default!;
    }

    public class VaccinationScheduleResponseDto
    {
        public Guid ScheduleId { get; set; }
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? ParentNotificationStartDate { get; set; }
        public DateOnly? ParentNotificationEndDate { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
