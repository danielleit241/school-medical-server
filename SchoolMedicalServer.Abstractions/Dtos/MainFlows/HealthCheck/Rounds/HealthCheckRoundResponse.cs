namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Rounds
{
    public class HealthCheckRoundResponse
    {
        public HealthCheckRoundInformationResponse HealthCheckRoundInformation { get; set; } = new();
        public HealthCheckRoundNurseInformationResponse Nurse { get; set; } = new();
    }
}
