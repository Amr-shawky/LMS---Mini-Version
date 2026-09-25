namespace exam_system.Features.Shared;

public class PaginatedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedResult() { }

    public PaginatedResult(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public static PaginatedResult<T> Create(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
        => new(items, count, pageIndex, pageSize);
}
