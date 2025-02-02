using MSIHORG.Common.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MSIHORG.Common.Server.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
        Task SendTemplatedEmailAsync(string to, string templateId, object templateData);
        Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body);
        Task SendEmailWithAttachmentAsync(string to, string subject, string body,
            IEnumerable<EmailAttachment> attachments);
    }
}
