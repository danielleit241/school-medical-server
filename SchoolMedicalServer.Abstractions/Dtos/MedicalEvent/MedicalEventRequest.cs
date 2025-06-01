using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalEvent
{
    public class MedicalEventRequest
    {
        [Required(ErrorMessage = "Medical event is required.")]
        public MedicalEventDtoRequest MedicalEvent { get; set; } = default!;
        [Required(ErrorMessage = "Medical requests are required.")]
        public List<MedicalRequestDtoRequest>? MedicalRequests { get; set; } = [];
    }

    public class MedicalEventDtoRequest
    {
        [Required(ErrorMessage = "Student code is required.")]
        public string StudentCode { get; set; } = default!;

        [Required(ErrorMessage = "Staff nurse ID is required.")]
        public Guid StaffNurseId { get; set; }

        [Required(ErrorMessage = "Event type is required.")]
        public string EventType { get; set; } = default!;

        [Required(ErrorMessage = "Event description is required.")]
        public string? EventDescription { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = default!;

        [Required(ErrorMessage = "Severity level is required.")]
        public string SeverityLevel { get; set; } = default!;

        [Required(ErrorMessage = "Parent notified is required.")]
        public bool ParentNotified { get; set; } = false;

        public string? Notes { get; set; }
    }

    public class MedicalRequestDtoRequest
    {
        [Required(ErrorMessage = "Item ID is required.")]
        public Guid ItemId { get; set; }

        [Required(ErrorMessage = "Request quantity is required.")]
        public int? RequestQuantity { get; set; }

        [Required(ErrorMessage = "Purpose is required.")]
        public string? Purpose { get; set; }
    }
}
