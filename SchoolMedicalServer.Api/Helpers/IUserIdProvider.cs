using Microsoft.AspNetCore.SignalR;

namespace SchoolMedicalServer.Api.Helpers
{
    public interface IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection);
    }
}
