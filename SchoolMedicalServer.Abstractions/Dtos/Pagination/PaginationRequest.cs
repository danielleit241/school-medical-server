using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationRequest
    {
        [Range(0, int.MaxValue)]
        public int PageSize { get; set; } = 10;

        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        public PaginationRequest() { }

    }
}
