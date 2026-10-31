
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace exam_system.Features.Shared
{
    public class PagedResult<T>(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
    {

        /// <param name="items">
        /// The read-only collection containing the data records for the current page scope. 
        /// Using <see cref="IReadOnlyList{T}"/> ensures memory-efficient index access <c>[i]</c> 
        /// and instantaneous <c>.Count</c> operations while preventing downstream array mutations.
        /// </param>
        public IReadOnlyList<T> Items { get; } = items;
        public int PageNumber { get; } = pageNumber;
        public int PageSize { get; } = pageSize;
        public int TotalCount { get; } = totalCount;

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    //Used The Extension Method ToPagedResultAsync() In The Following Way:
    //.Select(p => new ProjectListItem(p.Id, p.Name, p.Status))   
    //.OrderBy(p => p.Name)                                        
    //.ToPagedResultAsync(pageNumber, pageSize, ct);
    public static class PagingExtensions
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 20;

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize = DefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            pageSize = Math.Clamp(pageSize, 1, MaxPageSize);
            pageNumber = Math.Clamp(pageNumber, 1, int.MaxValue / MaxPageSize);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>(items, pageNumber, pageSize, totalCount);
        }
    }
}
