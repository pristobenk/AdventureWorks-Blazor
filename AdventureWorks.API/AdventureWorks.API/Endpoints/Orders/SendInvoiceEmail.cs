using AdventureWorks.Application.Abstractions.Email;
using AdventureWorks.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AdventureWorks.WebApi.Endpoints.Orders;

internal sealed class SendInvoiceEmail : IEndpoint
{
    public sealed class Request
    {
        public string ToEmail { get; set; } = string.Empty;
        public string? CcEmail { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string HtmlBody { get; set; } = string.Empty;
        public string? AttachmentFileName { get; set; }
        public string? AttachmentContent { get; set; } // base64
        public string? AttachmentContentType { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders/{orderId:int}/email", async (
            int orderId,
            Request request,
            IEmailSender emailSender,
            CancellationToken cancellationToken) =>
        {
            var attachments = new List<EmailAttachment>();
            if (!string.IsNullOrWhiteSpace(request.AttachmentContent) && !string.IsNullOrWhiteSpace(request.AttachmentFileName))
            {
                var content = Convert.FromBase64String(request.AttachmentContent);
                attachments.Add(new EmailAttachment
                {
                    FileName = request.AttachmentFileName,
                    Content = content,
                    ContentType = request.AttachmentContentType ?? "application/pdf"
                });
            }

            // If client did not provide attachment, generate invoice PDF server-side and attach it
            if (attachments.Count == 0)
            {
                // generate invoice PDF server-side
                var db = app.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var order = await db.Orders
                    .AsQueryable()
                    .Include(o => o.OrderLines)
                    .SingleOrDefaultAsync(o => o.OrderId == orderId, cancellationToken);

                if (order != null)
                {
                    var customer = await db.Customers.AsQueryable().SingleOrDefaultAsync(c => c.CustomerId == order.CustomerId, cancellationToken);
                    var productIds = order.OrderLines.Select(x => x.ProductId).Distinct().ToList();
                    var productsLookup = await db.Products.AsQueryable()
                        .Where(x => productIds.Contains(x.ProductId))
                        .Select(x => new { x.ProductId, x.Name, x.ProductNumber })
                        .ToDictionaryAsync(x => x.ProductId, cancellationToken);

                    var invoiceModel = new
                    {
                        Order = order,
                        Customer = customer,
                        Products = productsLookup
                    };

                    // Build a simple PDF using QuestPDF
                    var pdfBytes = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(2, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x.FontSize(10));

                            page.Header()
                                .Text($"Invoice INV-{order.OrderId}").SemiBold().FontSize(20);

                            page.Content().Column(col =>
                            {
                                col.Item().Text($"Customer: {customer?.CustomerName ?? order.CustomerId.ToString()}");
                                col.Item().Text($"Order Date: {order.OrderDate:dd MMM yyyy}");

                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(60);
                                        columns.RelativeColumn();
                                        columns.ConstantColumn(80);
                                        columns.ConstantColumn(80);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("ID");
                                        header.Cell().Text("Product");
                                        header.Cell().Text("Qty");
                                        header.Cell().Text("Line Total");
                                    });

                                    foreach (var l in order.OrderLines)
                                    {
                                        var p = productsLookup.GetValueOrDefault(l.ProductId);
                                        table.Cell().Text(l.ProductId.ToString());
                                        table.Cell().Text(p?.Name ?? $"Product #{l.ProductId}");
                                        table.Cell().Text(l.Quantity.ToString());
                                        table.Cell().Text((l.Quantity * l.UnitPrice).ToString("N2"));
                                    }
                                });

                                col.Item().AlignRight().Text($"Total: {order.TotalAmount:N2}").SemiBold();
                            });

                            page.Footer().AlignCenter().Text(x => { x.Span("Page "); x.CurrentPageNumber(); });
                        });
                    }).GeneratePdf();

                    attachments.Add(new EmailAttachment
                    {
                        FileName = $"invoice-{orderId}.pdf",
                        Content = pdfBytes,
                        ContentType = "application/pdf"
                    });
                }
            }

            await emailSender.SendAsync(
                request.ToEmail,
                request.Subject,
                request.HtmlBody,
                request.CcEmail,
                attachments.Count == 0 ? null : attachments.AsEnumerable(),
                cancellationToken);

            return Results.NoContent();
        })
        .WithTags(Tags.Orders)
        .RequireAuthorization();
    }
}
