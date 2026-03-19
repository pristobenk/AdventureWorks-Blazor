using AdventureWorks.Application.Abstractions.Data;
using AdventureWorks.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class PrintInvoice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{orderId:int}/invoice/pdf", async (
            int orderId,
            IApplicationDbContext context,
            CancellationToken cancellationToken) =>
        {
            var order = await context.Orders
                .AsNoTracking()
                .Include(x => x.OrderLines)
                .SingleOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);

            if (order is null)
            {
                return Results.NotFound();
            }

            var customer = await context.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CustomerId == order.CustomerId, cancellationToken);

            var productIds = order.OrderLines
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var productsLookup = await context.Products
                .AsNoTracking()
                .Where(x => productIds.Contains(x.ProductId))
                .Select(x => new { x.ProductId, x.Name, x.ProductNumber })
                .ToDictionaryAsync(x => x.ProductId, cancellationToken);

            var invoiceModel = new InvoiceModel
            {
                InvoiceNumber = $"INV-{order.OrderId}",
                OrderDate = order.OrderDate,
                CustomerName = customer?.CustomerName ?? $"Customer #{order.CustomerId}",
                ContactName = customer?.ContactName,
                Address = customer?.Address,
                Items = order.OrderLines.Select(line =>
                {
                    var product = productsLookup.GetValueOrDefault(line.ProductId);
                    return new InvoiceItemModel
                    {
                        ProductId = line.ProductId,
                        ProductNumber = product?.ProductNumber ?? "-",
                        ProductName = product?.Name ?? $"Product #{line.ProductId}",
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice
                    };
                }).ToList()
            };

            var pdfBytes = new InvoiceDocument(invoiceModel).GeneratePdf();

            return Results.File(
                pdfBytes,
                "application/pdf",
                $"invoice-{order.OrderId}.pdf");
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }

    private sealed class InvoiceDocument(InvoiceModel model) : IDocument
    {
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Column(column =>
                    {
                        column.Item().Text("INVOICE").SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);
                        column.Item().Text($"Invoice No: {model.InvoiceNumber}");
                        column.Item().Text($"Order Date: {model.OrderDate:dd MMM yyyy HH:mm}");
                    });

                page.Content()
                    .PaddingVertical(15)
                    .Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(c =>
                        {
                            c.Item().Text("Bill To").SemiBold();
                            c.Item().Text(model.CustomerName);

                            if (!string.IsNullOrWhiteSpace(model.ContactName))
                                c.Item().Text(model.ContactName);

                            if (!string.IsNullOrWhiteSpace(model.Address))
                                c.Item().Text(model.Address);
                        });

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(55);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(4);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(90);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("ID").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Product #").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Product Name").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Qty").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Unit Price").SemiBold();
                                header.Cell().Background(Colors.Grey.Lighten3).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text("Line Total").SemiBold();
                            });

                            foreach (var item in model.Items)
                            {
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item.ProductId.ToString());
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item.ProductNumber);
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item.ProductName);
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item.Quantity.ToString());
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item.UnitPrice.ToString("N2"));
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten3).Padding(6).Text(item.LineTotal.ToString("N2"));
                            }
                        });

                        column.Item().AlignRight().Text($"Total: {model.TotalAmount:N2}").SemiBold().FontSize(12);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        }

    }

    private sealed class InvoiceModel
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? ContactName { get; set; }
        public string? Address { get; set; }
        public List<InvoiceItemModel> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(x => x.LineTotal);
    }

    private sealed class InvoiceItemModel
    {
        public int ProductId { get; set; }
        public string ProductNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
