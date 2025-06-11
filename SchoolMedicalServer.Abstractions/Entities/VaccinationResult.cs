namespace SchoolMedicalServer.Abstractions.Entities
{
    public class VaccinationResult
    {
        public Guid VaccinationResultId { get; set; }
        public Guid RoundId { get; set; }
        public Guid HealthProfileId { get; set; }

        public bool ParentConfirmed { get; set; }
        public bool HealthQualified { get; set; } //bỏ đi
        public bool Vaccinated { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public Guid RecorderId { get; set; }

        //string status -> đã tiêm, chưa tiêm, vắng mặt, ko đủ điều kiện dự trên khai báo
        //string reason
        public string? Notes { get; set; }

        public virtual VaccinationRound? Round { get; set; }
        public virtual HealthProfile? HealthProfile { get; set; }
        public virtual VaccinationObservation? VaccinationObservation { get; set; }
    }
}