using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Infrastructure.Settings
{
    public class TwilioSettings
    {
        public const string SectionName = "TwilioSettings";

        /// <summary>
        /// Your Twilio Account SID
        /// </summary>
        public string AccountSid { get; set; } = string.Empty;

        /// <summary>
        /// Your Twilio Auth Token
        /// </summary>
        public string AuthToken { get; set; } = string.Empty;

        /// <summary>
        /// Your Twilio Phone Number
        /// </summary>
        public string FromNumber { get; set; } = string.Empty;

        /// <summary>
        /// Optional: Message Service SID for enhanced deliverability
        /// </summary>
        public string? MessageServiceSid { get; set; }

        /// <summary>
        /// Optional: Enable detailed logging for debugging
        /// </summary>
        public bool EnableDetailedLogging { get; set; }

        /// <summary>
        /// Validate required settings
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(AccountSid))
                throw new InvalidOperationException($"{nameof(AccountSid)} is required");

            if (string.IsNullOrWhiteSpace(AuthToken))
                throw new InvalidOperationException($"{nameof(AuthToken)} is required");

            if (string.IsNullOrWhiteSpace(FromNumber))
                throw new InvalidOperationException($"{nameof(FromNumber)} is required");
        }
    }
}
