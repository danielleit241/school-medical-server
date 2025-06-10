namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccineDetail
{
    public Guid VaccineId { get; set; }

    public string VaccineCode { get; set; } = default!;

    public string? VaccineName { get; set; }

    public string? Manufacturer { get; set; }

    public string? VaccineType { get; set; }

    public string? AgeRecommendation { get; set; }

    public string? BatchNumber { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public int? DoseNumber { get; set; }

    public string? ContraindicationNotes { get; set; }

    public string? Description { get; set; }

    public virtual VaccinationSchedule? VaccinationSchedule { get; set; }
}
