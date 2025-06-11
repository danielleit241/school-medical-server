using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.VaccinationDetails;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public class VaccinationDetailsService(IVacctionDetailsRepository vacctionDetailsRepository, IBaseRepository baseRepository) : IVaccinationDetailsService
    {
        public async Task<bool> CreateVaccineDetailAsync(VaccinationDetailsRequest vaccineDetail)
        {
            if (vaccineDetail == null || string.IsNullOrWhiteSpace(vaccineDetail.VaccineCode) || string.IsNullOrWhiteSpace(vaccineDetail.VaccineName))
            {
                return false;
            }
            var isExists = await vacctionDetailsRepository.IsExistsAsync(vaccineDetail!.VaccineCode!);
            if (isExists)
            {
                return false;
            }
            var newVaccineDetail = new VaccinationDetail
            {
                VaccineId = Guid.NewGuid(),
                VaccineCode = vaccineDetail.VaccineCode,
                VaccineName = vaccineDetail.VaccineName,
                Description = vaccineDetail.Description,
                Manufacturer = vaccineDetail.Manufacturer,
                VaccineType = vaccineDetail.VaccineType,
                AgeRecommendation = vaccineDetail.AgeRecommendation,
                BatchNumber = vaccineDetail.BatchNumber,
                ExpirationDate = vaccineDetail.ExpirationDate,
                ContraindicationNotes = vaccineDetail.ContraindicationNotes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await vacctionDetailsRepository.AddAsync(newVaccineDetail);
            await baseRepository.SaveChangesAsync();
            return true;
        }

        public async Task<VaccinationDetailsResponse> GetVaccineDetailAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null!;
            }
            var vaccineDetail = await vacctionDetailsRepository.GetByIdAsync(id);
            if (vaccineDetail == null)
            {
                return null!;
            }
            return MapToResponse(vaccineDetail);
        }

        public async Task<PaginationResponse<VaccinationDetailsResponse>> GetVaccineDetailsAsync(PaginationRequest? pagination)
        {
            var total = await vacctionDetailsRepository.CountAsync();
            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var vaccineDetails = await vacctionDetailsRepository.GetPagedAsync(
                pagination.Search!,
                pagination.SortBy!,
                pagination.SortOrder!,
                skip, pagination.PageSize);
            var response = vaccineDetails.Select(v => MapToResponse(v)).ToList();
            return new PaginationResponse<VaccinationDetailsResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                total,
                response
            );
        }

        public async Task<bool> UpdateVaccineDetailAsync(Guid id, VaccinationDetailsRequest vaccineDetail)
        {
            var vaccineDetailToUpdate = await vacctionDetailsRepository.GetByIdAsync(id);
            if (vaccineDetailToUpdate == null || vaccineDetail == null)
            {
                return false;
            }
            vaccineDetailToUpdate.VaccineCode = vaccineDetail!.VaccineCode!;
            vaccineDetailToUpdate.VaccineName = vaccineDetail.VaccineName;
            vaccineDetailToUpdate.Description = vaccineDetail.Description;
            vaccineDetailToUpdate.ExpirationDate = vaccineDetail.ExpirationDate;
            vaccineDetailToUpdate.BatchNumber = vaccineDetail.BatchNumber;
            vaccineDetailToUpdate.AgeRecommendation = vaccineDetail.AgeRecommendation;
            vaccineDetailToUpdate.ContraindicationNotes = vaccineDetail.ContraindicationNotes;
            vaccineDetailToUpdate.Manufacturer = vaccineDetail.Manufacturer;
            vaccineDetailToUpdate.VaccineType = vaccineDetail.VaccineType;
            vaccineDetail.Description = vaccineDetail.Description;
            vaccineDetailToUpdate.UpdatedAt = DateTime.UtcNow;
            vacctionDetailsRepository.Update(vaccineDetailToUpdate);
            await baseRepository.SaveChangesAsync();
            return true;
        }

        private VaccinationDetailsResponse MapToResponse(VaccinationDetail detail)
        {
            return new VaccinationDetailsResponse
            {
                VaccineId = detail.VaccineId,
                VaccineCode = detail.VaccineCode,
                VaccineName = detail.VaccineName,
                Description = detail.Description,
                Manufacturer = detail.Manufacturer,
                VaccineType = detail.VaccineType,
                AgeRecommendation = detail.AgeRecommendation,
                BatchNumber = detail.BatchNumber,
                ExpirationDate = detail.ExpirationDate,
                ContraindicationNotes = detail.ContraindicationNotes,
                CreateAt = detail.CreatedAt,
                UpdateAt = detail.UpdatedAt
            };
        }
    }
}
