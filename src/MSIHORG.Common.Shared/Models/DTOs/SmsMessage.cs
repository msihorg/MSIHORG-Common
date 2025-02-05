using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Shared.Models.DTOs
{
    public class SmsMessage
    {
        public string To { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? From { get; set; }
    }
}
