namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.Vaccination.Schedules
{
    public class VaccinationScheduleUpdateRequest
    {
        public Guid? ScheduleId { get; set; }
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
