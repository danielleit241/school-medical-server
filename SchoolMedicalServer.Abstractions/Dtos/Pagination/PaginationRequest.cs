using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationRequest
    {
        [Range(10, 40)]
        public int PageSize { get; set; } = 10;

        [Range(1, 100)]
        public int PageIndex { get; set; } = 1;

        public PaginationRequest() { }

    }
}
