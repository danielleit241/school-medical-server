using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IMedicalRegistrationDetailsRepository
    {
        Task<MedicalRegistrationDetails?> GetDetailsByRegistrationAndDoseAsync(Guid registrationId, string doseNumber);
        Task<List<MedicalRegistrationDetails>> GetDetailsByRegistrationIdAsync(Guid registrationId);
        void UpdateDetails(MedicalRegistrationDetails details);
        Task<MedicalRegistrationDetails?> GetByIdWithRegistrationAndStudentAsync(Guid medicalRegistrationDetailsId);
    }
}

