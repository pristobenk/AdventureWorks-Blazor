using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.Application.Abstractions.Authentication;
using AdventureWorks.Domain.Products;
using AdventureWorks.SharedKernel;

namespace AdventureWorks.Application.Products.Create;

internal sealed class CreateProductCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateProductCommand, int>
{
    public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            ProductNumber = command.ProductNumber,
            MakeFlag = command.MakeFlag,
            FinishedGoodsFlag = command.FinishedGoodsFlag,
            Color = command.Color,
            SafetyStockLevel = command.SafetyStockLevel,
            ReorderPoint = command.ReorderPoint,
            StandardCost = command.StandardCost,
            ListPrice = command.ListPrice,
            Size = command.Size,
            SizeUnitMeasureCode = command.SizeUnitMeasureCode,
            WeightUnitMeasureCode = command.WeightUnitMeasureCode,
            Weight = command.Weight,
            DaysToManufacture = command.DaysToManufacture,
            ProductLine = command.ProductLine,
            Class = command.Class,
            Style = command.Style,
            SellStartDate = command.SellStartDate,
            SellEndDate = command.SellEndDate,
            DiscontinuedDate = command.DiscontinuedDate,
            Rowguid = Guid.NewGuid(),
            ModifiedDate = dateTimeProvider.UtcNow
        };

        context.Products.Add(product);

        await context.SaveChangesAsync(cancellationToken);

        return product.ProductId;
    }
}
