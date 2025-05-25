using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccineDetail
{
    public Guid VaccineId { get; set; }

    public string? VaccineName { get; set; }

    public string? Manufacturer { get; set; }

    public string? Disease { get; set; }

    public string? VaccineType { get; set; }

    public string? AgeRecommendation { get; set; }

    public string? BatchNumber { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public int? DoseNumber { get; set; }

    public string? ContraindicationNotes { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<HealthDeclaration> HealthDeclarations { get; set; } = new List<HealthDeclaration>();

    public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; } = new List<VaccinationSchedule>();
}
