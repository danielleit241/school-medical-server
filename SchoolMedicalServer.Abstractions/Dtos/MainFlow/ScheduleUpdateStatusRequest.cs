namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow
{
    public class ScheduleUpdateStatusRequest
    {
        public Guid ScheduleId { get; set; }
        public bool Status { get; set; }
    }
}
