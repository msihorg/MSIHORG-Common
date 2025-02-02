using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using MSIHORG.Common.Server.Core.Services;
using MSIHORG.Common.Server.Infrastructure.Services.Settings;
using MSIHORG.Common.Shared.Models.DTOs;

namespace MSIHORG.Common.Server.Infrastructure.Services.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly SendGridSettings _settings;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(
            IOptions<SendGridSettings> settings, ILogger<SendGridEmailService> logger)
        {
            _settings = settings.Value;
            _client = new SendGridClient(_settings.ApiKey);
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                var msg = new SendGridMessage
                {
                    From = new EmailAddress(emailMessage.From ?? _settings.DefaultFromEmail, _settings.DefaultFromName),
                    Subject = emailMessage.Subject,
                    HtmlContent = emailMessage.Body
                };

                msg.AddTo(new EmailAddress(emailMessage.To));

                var response = await _client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    throw new EmailServiceException(
                        $"Failed to send email via SendGrid. Status: {response.StatusCode}, Body: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", emailMessage.To);
                throw;
            }
        }

        public async Task SendTemplatedEmailAsync(string to, string templateId, object templateData)
        {
            try
            {
                var msg = new SendGridMessage
                {
                    From = new EmailAddress(_settings.DefaultFromEmail, _settings.DefaultFromName),
                    TemplateId = templateId
                };

                msg.AddTo(new EmailAddress(to));
                msg.SetTemplateData(templateData);

                var response = await _client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    throw new EmailServiceException(
                        $"Failed to send templated email via SendGrid. Status: {response.StatusCode}, Body: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send templated email to {To} using template {TemplateId}",
                    to, templateId);
                throw;
            }
        }

        public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body)
        {
            try
            {
                // SendGrid recommends sending to no more than 1000 recipients per request
                const int batchSize = 1000;
                var recipientBatches = recipients.Chunk(batchSize);

                foreach (var batch in recipientBatches)
                {
                    var msg = new SendGridMessage
                    {
                        From = new EmailAddress(_settings.DefaultFromEmail, _settings.DefaultFromName),
                        Subject = subject,
                        HtmlContent = body
                    };

                    foreach (var recipient in batch)
                    {
                        msg.AddTo(new EmailAddress(recipient));
                    }

                    var response = await _client.SendEmailAsync(msg);
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Body.ReadAsStringAsync();
                        throw new EmailServiceException(
                            $"Failed to send bulk email via SendGrid. Status: {response.StatusCode}, Body: {responseBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send bulk email to {Count} recipients", recipients.Count());
                throw;
            }
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body,
            IEnumerable<EmailAttachment> attachments)
        {
            try
            {
                var msg = new SendGridMessage
                {
                    From = new EmailAddress(_settings.DefaultFromEmail, _settings.DefaultFromName),
                    Subject = subject,
                    HtmlContent = body
                };

                msg.AddTo(new EmailAddress(to));

                foreach (var attachment in attachments)
                {
                    var attachmentContent = Convert.ToBase64String(attachment.Content);
                    msg.AddAttachment(attachment.FileName, attachmentContent, attachment.ContentType);
                }

                var response = await _client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    throw new EmailServiceException(
                        $"Failed to send email with attachments via SendGrid. Status: {response.StatusCode}, Body: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with attachments to {To}", to);
                throw;
            }
        }
    }

    public class EmailServiceException : Exception
    {
        public EmailServiceException(string message) : base(message) { }
    }
}