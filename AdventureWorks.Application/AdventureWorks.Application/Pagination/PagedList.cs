namespace AdventureWorks.Application.Pagination;

public class PagedList<T>
{
    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;

    public PagedList(List<T> items, int count, int page, int pageSize)
    {
        Items = items;
        TotalCount = count;
        Page = page;
        PageSize = pageSize;
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(source, cancellationToken);
        var items = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
            source.Skip((page - 1) * pageSize).Take(pageSize), cancellationToken);

        return new PagedList<T>(items, count, page, pageSize);
    }
}
