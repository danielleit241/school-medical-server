namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.HealthCheck.Rounds
{
    public class HealthCheckRoundResponse
    {
        public HealthCheckRoundInformationResponse HealthCheckRoundInformation { get; set; } = new();
        public HealthCheckRoundNurseInformationResponse Nurse { get; set; } = new();
    }
}
