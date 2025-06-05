using Microsoft.AspNetCore.SignalR;

namespace SchoolMedicalServer.Api.Provider
{
    public interface IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection);
    }
}
