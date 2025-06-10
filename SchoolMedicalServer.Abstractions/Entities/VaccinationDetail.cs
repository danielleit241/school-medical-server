namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccinationDetail
{
    public Guid VaccineId { get; set; } // Đổi tên property cho đúng với entity mới

    public string VaccineCode { get; set; } = default!;

    public string? VaccineName { get; set; }

    public string? Manufacturer { get; set; }

    public string? VaccineType { get; set; }

    public string? AgeRecommendation { get; set; }

    public string? BatchNumber { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public string? ContraindicationNotes { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; } = new List<VaccinationSchedule>();
}