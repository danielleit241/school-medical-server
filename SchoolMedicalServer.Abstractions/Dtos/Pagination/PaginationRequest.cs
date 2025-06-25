using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationRequest
    {
        [Range(3, 40)]
        public int PageSize { get; set; } = 10;

        [Range(1, 100)]
        public int PageIndex { get; set; } = 1;

        public string? Search { get; set; }

        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";

        public PaginationRequest() { }

    }
}
