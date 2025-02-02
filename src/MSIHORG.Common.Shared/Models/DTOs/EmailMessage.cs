using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Shared.Models.DTOs
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string? From { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
        public IEnumerable<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }
}
