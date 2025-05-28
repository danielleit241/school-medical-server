using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.Student
{
    public class StudentDTO
    {
        public Guid StudentId { get; set; }
        public string? StudentCode { get; set; }
        public string FullName { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string AvatarURL { get; set; } = default!;
        public string? Gender { get; set; }

        public string? Grade { get; set; }

        public string? Address { get; set; }

        public string? ParentPhoneNumber { get; set; }

        public string? ParentEmailAddress { get; set; }
    }
}
