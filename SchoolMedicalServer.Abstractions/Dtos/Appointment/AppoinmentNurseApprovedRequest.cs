using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.Appointment
{
    public class AppoinmentNurseApprovedRequest
    {
        public Guid StaffNurseId { get; set; }

        public bool? ConfirmationStatus { get; set; }

        public bool? CompletionStatus { get; set; }
    }
}
