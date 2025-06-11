namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination
{
    public class ParentVaccinationConfirmationRequest
    {
        public Guid UserId { get; set; }
        public bool status { get; set; }
    }
}
