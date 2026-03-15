namespace LMS___Mini_Version.Features.Shared
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public PaginatedResult(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Page = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPrevious = Page > 1;
            HasNext = Page < TotalPages;
            Items = items;
        }
    }
}
