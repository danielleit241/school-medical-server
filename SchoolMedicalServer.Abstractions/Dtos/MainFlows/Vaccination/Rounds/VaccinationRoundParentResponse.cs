namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Rounds
{
    public class VaccinationRoundParentResponse
    {
        public VaccinationRoundInformationResponse VaccinationRoundInformation { get; set; } = new VaccinationRoundInformationResponse();
        public VaccinationRoundNurseInformationResponse Nurse { get; set; } = new VaccinationRoundNurseInformationResponse();
        public StudentsOfRoundResponse Student { get; set; } = new StudentsOfRoundResponse();
        public ParentOfStudentResponse Parent { get; set; } = new ParentOfStudentResponse();
    }
}
