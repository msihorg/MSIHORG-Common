using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Shared.Models.DTOs
{
    public class PasswordlessLoginRequest
    {
        public string Contact { get; set; } = string.Empty;
        public string ContactType { get; set; } = string.Empty; // "Email" or "Phone"
    }
}
