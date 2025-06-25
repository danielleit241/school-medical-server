namespace SchoolMedicalServer.Api.Helpers
{
    public interface INameUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection);
    }
}
