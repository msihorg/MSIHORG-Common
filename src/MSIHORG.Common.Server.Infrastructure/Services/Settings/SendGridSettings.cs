﻿namespace MSIHORG.Common.Server.Infrastructure.Services.Settings
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string DefaultFromEmail { get; set; } = string.Empty;
        public string DefaultFromName { get; set; } = string.Empty;
    }
}
