namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class ScheduleUpdateStatusRequest
    {
        public Guid ScheduleId { get; set; }
        public bool Status { get; set; }
    }
}
