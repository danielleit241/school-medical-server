namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationRequest(int pageSize = 10, int pageIndex = 1)
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

        public PaginationRequest() { }

    }
}
