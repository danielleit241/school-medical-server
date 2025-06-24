using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows
{
    public class ScheduleUpdateStatusRequest
    {
        [Required]
        public Guid ScheduleId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
