using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace SchoolMedicalServer.Api.Helpers
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var claim = connection.User?.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value!;
        }
    }
}
