namespace SchoolMedicalServer.Abstractions.Dtos.MedicalEvent
{
    public class MedicalRequestResponse
    {
        public EventInfo EventInfo { get; set; } = new();
        public NurseInfo NurseInfo { get; set; } = new();
        public MedicalInfo MedicalInfo { get; set; } = new();
    }

    public class EventInfo
    {
        public Guid EventId { get; set; }
    }

    public class NurseInfo
    {
        public Guid NurseId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }

    public class MedicalInfo
    {
        public Guid RequestId { get; set; }
        public Guid? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? RequestQuantity { get; set; }
        public DateOnly? RequestDate { get; set; }
    }
}
