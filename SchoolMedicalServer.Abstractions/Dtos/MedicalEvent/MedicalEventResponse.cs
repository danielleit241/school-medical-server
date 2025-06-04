namespace SchoolMedicalServer.Abstractions.Dtos.MedicalEvent
{
    public class MedicalEventResponse
    {
        public MedicalEventDtoResponse? MedicalEvent { get; set; }
        public StudentInforResponse? StudentInfo { get; set; }
        public List<MedicalRequestDtoResponse>? MedicalRequests { get; set; } = [];
    }

    public class MedicalEventDtoResponse
    {
        public Guid EventId { get; set; }
        public Guid? StaffNurseId { get; set; }
        public DateOnly? EventDate { get; set; }
        public string? EventType { get; set; }
        public string? EventDescription { get; set; }
        public string? Location { get; set; }
        public string? SeverityLevel { get; set; }
        public string? Notes { get; set; }
    }

    public class StudentInforResponse
    {
        public Guid? StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }

    public class MedicalRequestDtoResponse
    {
        public Guid RequestId { get; set; }
        public Guid? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? RequestQuantity { get; set; }
    }
}
