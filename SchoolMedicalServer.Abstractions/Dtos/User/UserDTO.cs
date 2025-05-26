using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.User
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public int? RoleId { get; set; }

        public string? FullName { get; set; }


        public string? EmailAddress { get; set; }

        public string? AvatarUrl { get; set; }

        public DateOnly? DayOfBirth { get; set; }
    }
}
