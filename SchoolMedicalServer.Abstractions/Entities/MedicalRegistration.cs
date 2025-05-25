using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class MedicalRegistration
{
    public Guid RegistrationId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? UserId { get; set; }

    public DateOnly? DateSubmitted { get; set; }

    public string? MedicationName { get; set; }

    public string? Dosage { get; set; }

    public string? Notes { get; set; }

    public bool? ParentalConsent { get; set; }

    public bool? Status { get; set; }

    public virtual Student? Student { get; set; }

    public virtual User? User { get; set; }
}
