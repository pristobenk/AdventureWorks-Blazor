using AdventureWorks.Application.Abstractions.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AdventureWorks.Infrastructure.Email;

internal sealed class SmtpEmailSender(IOptions<SmtpSettings> options) : IEmailSender
{
    private readonly SmtpSettings settings = options.Value;

    public async Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? ccEmail = null,
        IEnumerable<EmailAttachment>? attachments = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(settings.Host))
        {
            throw new InvalidOperationException("SMTP host is not configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.FromEmail))
        {
            throw new InvalidOperationException("SMTP FromEmail is not configured.");
        }

        using var message = new MailMessage();
        message.From = new MailAddress(settings.FromEmail, settings.FromName);
        message.To.Add(toEmail);

        if (!string.IsNullOrWhiteSpace(ccEmail))
        {
            message.CC.Add(ccEmail);
        }

        message.Subject = subject;
        message.Body = htmlBody;
        message.IsBodyHtml = true;

        if (attachments is not null)
        {
            foreach (var a in attachments)
            {
                var ms = new System.IO.MemoryStream(a.Content);
                var attach = new Attachment(ms, a.FileName, a.ContentType);
                // Ensure the stream is disposed when message is disposed
                attach.ContentStream.Position = 0;
                message.Attachments.Add(attach);
            }
        }

        using var client = new SmtpClient(settings.Host, settings.Port)
        {
            EnableSsl = settings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };

        if (!string.IsNullOrWhiteSpace(settings.UserName))
        {
            client.Credentials = new NetworkCredential(settings.UserName, settings.Password);
        }

        cancellationToken.ThrowIfCancellationRequested();
        await client.SendMailAsync(message, cancellationToken);
    }
}
