using AdventureWorks.Application.Abstractions.Messaging;
using AdventureWorks.Application.Products.Create;
using AdventureWorks.SharedKernel;
using AdventureWorks.WebApi.Extensions;
using AdventureWorks.WebApi.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Products;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string Name { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public bool MakeFlag { get; set; }
        public bool FinishedGoodsFlag { get; set; }
        public string? Color { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public string? SizeUnitMeasureCode { get; set; }
        public string? WeightUnitMeasureCode { get; set; }
        public decimal? Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string? ProductLine { get; set; }
        public string? Class { get; set; }
        public string? Style { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
            Request request,
            ICommandHandler<CreateProductCommand, int> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateProductCommand
            {
                Name = request.Name,
                ProductNumber = request.ProductNumber,
                MakeFlag = request.MakeFlag,
                FinishedGoodsFlag = request.FinishedGoodsFlag,
                Color = request.Color,
                SafetyStockLevel = request.SafetyStockLevel,
                ReorderPoint = request.ReorderPoint,
                StandardCost = request.StandardCost,
                ListPrice = request.ListPrice,
                Size = request.Size,
                SizeUnitMeasureCode = request.SizeUnitMeasureCode,
                WeightUnitMeasureCode = request.WeightUnitMeasureCode,
                Weight = request.Weight,
                DaysToManufacture = request.DaysToManufacture,
                ProductLine = request.ProductLine,
                Class = request.Class,
                Style = request.Style,
                SellStartDate = request.SellStartDate,
                SellEndDate = request.SellEndDate,
                DiscontinuedDate = request.DiscontinuedDate
            };

            Result<int> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Products)
        .RequireAuthorization();
    }
}

