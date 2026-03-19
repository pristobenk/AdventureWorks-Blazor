using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Web.Models;

public sealed class SendInvoiceEmailRequest
{
    [Required, EmailAddress]
    public string ToEmail { get; set; } = string.Empty;

    [EmailAddress]
    public string? CcEmail { get; set; }

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string HtmlBody { get; set; } = string.Empty;
}
