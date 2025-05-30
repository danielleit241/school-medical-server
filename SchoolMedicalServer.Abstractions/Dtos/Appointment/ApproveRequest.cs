using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.Appointment
{
    public class ApproveRequest
    {
        public bool? ConfirmationStatus { get; set; }

        public bool? CompletionStatus { get; set; }
    }
}
