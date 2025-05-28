namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationRequest
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

        public PaginationRequest() { }

    }
}
