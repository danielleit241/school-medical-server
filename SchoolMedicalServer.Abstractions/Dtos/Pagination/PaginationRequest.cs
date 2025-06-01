using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationRequest
    {
        [Range(0, 40)]
        public int PageSize { get; set; } = 10;

        [Range(0, 100)]
        public int PageIndex { get; set; } = 1;

        public PaginationRequest() { }

    }
}
