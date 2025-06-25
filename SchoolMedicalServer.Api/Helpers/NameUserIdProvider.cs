namespace SchoolMedicalServer.Api.Helpers
{
    public class NameUserIdProvider : INameUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var claim = connection.User?.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value!;
        }
    }
}
