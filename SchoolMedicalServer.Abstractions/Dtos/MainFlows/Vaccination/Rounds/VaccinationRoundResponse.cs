namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Rounds
{
    public class VaccinationRoundResponse
    {
        public VaccinationRoundInformationResponse VaccinationRoundInformation { get; set; } = new VaccinationRoundInformationResponse();
        public VaccinationRoundNurseInformationResponse Nurse { get; set; } = new VaccinationRoundNurseInformationResponse();
    }

    public class VaccinationRoundInformationResponse
    {
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public string TargetGrade { get; set; } = string.Empty;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
    public class VaccinationRoundNurseInformationResponse
    {
        public Guid NurseId { get; set; }
        public string NurseName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
