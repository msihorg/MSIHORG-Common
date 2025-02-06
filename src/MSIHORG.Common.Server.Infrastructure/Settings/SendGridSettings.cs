namespace MSIHORG.Common.Server.Infrastructure.Settings
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrEmpty(ApiKey))
                throw new ArgumentNullException(nameof(ApiKey), "SendGrid API key is required");

            if (string.IsNullOrEmpty(FromEmail))
                throw new ArgumentNullException(nameof(FromEmail), "Sender email is required");

            if (string.IsNullOrEmpty(FromName))
                throw new ArgumentNullException(nameof(FromName), "Sender name is required");
        }
    }
}
