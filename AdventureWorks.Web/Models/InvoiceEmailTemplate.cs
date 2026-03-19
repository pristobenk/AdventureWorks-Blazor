namespace AdventureWorks.Web.Models;

public sealed class InvoiceEmailTemplateResult
{
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}

public static class InvoiceEmailTemplateBuilder
{
    public static InvoiceEmailTemplateResult Build(GetOrderByIdResponse order, CustomerDto customer, DateTime dueDate)
    {
        var customerName = System.Net.WebUtility.HtmlEncode(customer.CustomerName);
        var contactName = System.Net.WebUtility.HtmlEncode(customer.ContactName ?? string.Empty);
        var address = System.Net.WebUtility.HtmlEncode(customer.Address ?? string.Empty);

        var subject = $"Invoice #{order.OrderId} - Jatuh Tempo {dueDate:dd MMM yyyy}";

        var html = $$"""
<!DOCTYPE html>
<html lang="id">
<head>
    <meta charset="utf-8" />
    <style>
        body { font-family: Arial, Helvetica, sans-serif; color: #222; line-height: 1.6; }
        .container { max-width: 720px; margin: 0 auto; padding: 24px; border: 1px solid #e5e7eb; border-radius: 12px; }
        .header { border-bottom: 1px solid #e5e7eb; padding-bottom: 12px; margin-bottom: 20px; }
        .title { font-size: 24px; font-weight: 700; margin: 0; }
        .meta { color: #6b7280; font-size: 14px; }
        .summary { background: #f9fafb; border: 1px solid #e5e7eb; padding: 16px; border-radius: 10px; }
        .summary-table { width: 100%; border-collapse: collapse; margin-top: 12px; }
        .summary-table td { padding: 6px 0; vertical-align: top; }
        .label { color: #6b7280; width: 180px; }
        .amount { font-size: 18px; font-weight: 700; color: #111827; }
        .footer { margin-top: 24px; font-size: 13px; color: #6b7280; }
        .due { color: #b45309; font-weight: 700; }
        .btn { display: inline-block; margin-top: 16px; background: #2563eb; color: #fff !important; text-decoration: none; padding: 10px 16px; border-radius: 8px; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <p class="title">Invoice Reminder</p>
            <div class="meta">Invoice #{{order.OrderId}} • Tanggal Invoice: {{order.OrderDate:dd MMM yyyy}}</div>
        </div>

        <p>Halo <strong>{{customerName}}</strong>,</p>
        <p>Berikut adalah invoice untuk pesanan Anda. Mohon lakukan pembayaran sebelum tanggal jatuh tempo.</p>

        <div class="summary">
            <table class="summary-table">
                <tr>
                    <td class="label">Customer</td>
                    <td><strong>{{customerName}}</strong></td>
                </tr>
                <tr>
                    <td class="label">Contact</td>
                    <td>{{contactName}}</td>
                </tr>
                <tr>
                    <td class="label">Address</td>
                    <td>{{address}}</td>
                </tr>
                <tr>
                    <td class="label">Invoice Number</td>
                    <td>INV-{{order.OrderId}}</td>
                </tr>
                <tr>
                    <td class="label">Total Invoice</td>
                    <td class="amount">{{order.TotalAmount:N2}}</td>
                </tr>
                <tr>
                    <td class="label">Jatuh Tempo</td>
                    <td class="due">{{dueDate:dd MMM yyyy}}</td>
                </tr>
            </table>
        </div>

        <p>Jika Anda sudah melakukan pembayaran, silakan abaikan email ini.</p>
        <p>Terima kasih atas perhatian dan kerja samanya.</p>

        <div class="footer">
            <p>Hormat kami,<br />AdventureWorks</p>
        </div>
    </div>
</body>
</html>
""";

        return new InvoiceEmailTemplateResult
        {
            Subject = subject,
            HtmlBody = html,
            DueDate = dueDate
        };
    }
}
