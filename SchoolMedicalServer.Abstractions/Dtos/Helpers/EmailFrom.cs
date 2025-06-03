namespace SchoolMedicalServer.Abstractions.Dtos.Helpers
{
    public class EmailFrom
    {
        public string To { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
    }
}
