namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationNurseApprovedRequest
    {
        public Guid? StaffNurseId { get; set; }
        public string? StaffNurseNotes { get; set; }
        public DateOnly? DateApproved { get; set; }
    }
}
