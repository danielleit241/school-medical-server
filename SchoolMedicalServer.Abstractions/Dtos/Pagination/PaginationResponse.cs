namespace SchoolMedicalServer.Abstractions.Dtos.Pagination
{
    public class PaginationResponse<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> items)
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public long Count { get; set; } = count;
        public IEnumerable<TEntity> Items { get; set; } = items;
    }
}
