using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.UserProfile
{
    public class UserProfileResponse
    {
        public string FullName { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public DateOnly? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public string EmailAddress { get; set; } = default!;

    }
}
