namespace SchoolMedicalServer.Api.Hubs
{
    public class NotificationHub : Hub
    {
        [Authorize]
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("User connected: " + Context.UserIdentifier);
            return base.OnConnectedAsync();
        }
    }
}
