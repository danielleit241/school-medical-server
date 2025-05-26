using SchoolMedicalServer.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.Profile
{
    public class UserProfileDTO
    {
        public string FullName { get; set; } = default !;

        public DateOnly? DateOfBirth { get; set; }

        public string? AvatarURL { get; set; }

        public string Email { get; set; } = default !;

    }
}
