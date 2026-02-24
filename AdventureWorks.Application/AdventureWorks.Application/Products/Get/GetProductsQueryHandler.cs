using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Application.Products.Get;

internal sealed class GetProductsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetProductsQuery, List<GetProductsResponse>>
{
    public async Task<Result<List<GetProductsResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        List<GetProductsResponse> products = await context.Products
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

        return products;
    }
}

