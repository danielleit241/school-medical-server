using System.Text.Json.Serialization;

namespace SchoolMedicalServer.Abstractions.Entities
{
    public class MedicalRegistrationDetails
    {
        public Guid MedicalRegistrationDetailsId { get; set; }
        public Guid RegistrationId { get; set; }
        public Guid? StaffNurseId { get; set; }
        public string? DoseNumber { get; set; }
        public string? DoseTime { get; set; }
        public string? Notes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }

        [JsonIgnore]
        public virtual MedicalRegistration? MedicalRegistration { get; set; }
    }
}