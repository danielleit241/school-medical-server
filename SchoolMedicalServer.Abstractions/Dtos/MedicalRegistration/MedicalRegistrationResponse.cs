namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationResponse
    {
        public MedicalRegistrationDto? MedicalRegistration { get; set; }
        public MedicalRegistrationNurseApprovedResponse? NurseApproved { get; set; }
        public MedicalRegistrationStudentResponse? Student { get; set; }
        public MedicalRegistrationParentResponse? Parent { get; set; }
    }

    public class MedicalRegistrationDto
    {
        public Guid RegistrationId { get; set; }
        public string? MedicationName { get; set; }
        public string? Dosage { get; set; }
        public string? Notes { get; set; }
        public bool ParentConsent { get; set; }
        public DateOnly? DateSubmitted { get; set; }
    }

    public class MedicalRegistrationNurseApprovedResponse
    {
        public Guid? StaffNurseId { get; set; }
        public string? StaffNurseFullName { get; set; }
        public string? StaffNurseNotes { get; set; }
        public DateOnly? DateApproved { get; set; }
    }

    public class MedicalRegistrationStudentResponse
    {
        public Guid StudentId { get; set; }
        public string? StudentFullName { get; set; }
    }

    public class MedicalRegistrationParentResponse
    {
        public Guid? UserId { get; set; }
        public string? UserFullName { get; set; }
    }
}
