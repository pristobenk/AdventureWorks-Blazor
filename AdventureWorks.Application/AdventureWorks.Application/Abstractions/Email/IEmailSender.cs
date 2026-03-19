namespace AdventureWorks.Application.Abstractions.Email;

public interface IEmailSender
{
    Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? ccEmail = null,
        IEnumerable<EmailAttachment>? attachments = null,
        CancellationToken cancellationToken = default);
}

public sealed class EmailAttachment
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/octet-stream";
}
