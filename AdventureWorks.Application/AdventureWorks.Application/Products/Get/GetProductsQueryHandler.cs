using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Pagination;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Products.Get;

internal sealed class GetProductsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetProductsQuery, PagedList<GetProductsResponse>>
{
    public async Task<Result<PagedList<GetProductsResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // sanitize paging parameters
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var productsQuery = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            productsQuery = productsQuery.Where(p => p.Name.Contains(query.SearchTerm));
        }

        var totalCount = await productsQuery.CountAsync(cancellationToken);

        List<GetProductsResponse> products = await productsQuery
            .OrderBy(p => p.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new GetProductsResponse
            {
                ProductId = p.ProductId,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                MakeFlag = p.MakeFlag,
                FinishedGoodsFlag = p.FinishedGoodsFlag,
                Color = p.Color,
                SafetyStockLevel = p.SafetyStockLevel,
                ReorderPoint = p.ReorderPoint,
                StandardCost = p.StandardCost,
                ListPrice = p.ListPrice,
                Size = p.Size,
                SizeUnitMeasureCode = p.SizeUnitMeasureCode,
                WeightUnitMeasureCode = p.WeightUnitMeasureCode,
                Weight = p.Weight,
                DaysToManufacture = p.DaysToManufacture,
                ProductLine = p.ProductLine,
                Class = p.Class,
                Style = p.Style,
                SellStartDate = p.SellStartDate,
                SellEndDate = p.SellEndDate,
                DiscontinuedDate = p.DiscontinuedDate,
                Rowguid = p.Rowguid,
                ModifiedDate = p.ModifiedDate ?? DateTime.MinValue
            })
            .ToListAsync(cancellationToken);

        return new PagedList<GetProductsResponse>(products, totalCount, page, pageSize);
    }
}

