using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Helpers
{
    public class EmailFrom
    {
        [Required]
        public string To { get; set; } = default!;
        [Required]
        public string Subject { get; set; } = default!;
        [Required]
        public string Body { get; set; } = default!;
    }
}
