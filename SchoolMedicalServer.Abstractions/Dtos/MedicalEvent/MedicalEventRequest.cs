namespace SchoolMedicalServer.Abstractions.Dtos.MedicalEvent
{
    public class MedicalEventRequest
    {
        public MedicalEventDtoRequest MedicalEvent { get; set; } = default!;
        public List<MedicalRequestDtoRequest>? MedicalRequests { get; set; } = [];
    }

    public class MedicalEventDtoRequest
    {
        public string StudentCode { get; set; } = default!;
        public Guid StaffNurseId { get; set; }
        public string EventType { get; set; } = default!;
        public string? EventDescription { get; set; }
        public string Location { get; set; } = default!;
        public string SeverityLevel { get; set; } = default!;
        public bool ParentNotified { get; set; } = false;
        public string? Notes { get; set; }
    }

    public class MedicalRequestDtoRequest
    {
        public Guid ItemId { get; set; }
        public int? RequestQuantity { get; set; }
        public string? Purpose { get; set; }
    }
}
