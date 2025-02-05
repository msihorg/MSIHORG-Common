// BlazorTemplate.Infrastructure\Services\SMS\TwilioSmsService.cs
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio;
using Twilio.Clients;
using Task = System.Threading.Tasks.Task;
using MSIHORG.Common.Server.Core.Services;
using MSIHORG.Common.Server.Infrastructure.Settings;
using MSIHORG.Common.Shared.Models.DTOs;

namespace MSIHORG.Common.Server.Infrastructure.Services.Settings.SMS
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioRestClient _client;
        private readonly TwilioSettings _settings;
        private readonly ILogger<TwilioSmsService> _logger;

        public TwilioSmsService(
            IOptions<TwilioSettings> settings,
            ILogger<TwilioSmsService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            _client = new TwilioRestClient(_settings.AccountSid, _settings.AuthToken);
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        }

        public async Task SendMessageAsync(SmsMessage message)
        {
            try
            {
                var messageResource = await MessageResource.CreateAsync(
                    to: new PhoneNumber(message.To),
                    from: new PhoneNumber(message.From ?? _settings.FromNumber),
                    body: message.Message,
                    client: _client
                );

                if (messageResource.Status != MessageResource.StatusEnum.Sent &&
                    messageResource.Status != MessageResource.StatusEnum.Queued)
                {
                    throw new SmsServiceException(
                        $"Failed to send SMS. Status: {messageResource.Status}, Error: {messageResource.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SMS to {To}", message.To);
                throw;
            }
        }

        public async Task SendSmsAsync(SmsMessage message)
        {
            await SendMessageAsync(new SmsMessage
            {
                To = message.To,
                Message = message.Message,
                From = _settings.FromNumber
            });
        }

        public async Task SendTemplatedSmsAsync(string to, string templateId, object templateData)
        {
            try
            {
                // Convert template data to dictionary if not already
                var templateVars = templateData as IDictionary<string, string>
                    ?? ConvertToStringDictionary(templateData);

                // Get template content - In a real implementation, you would likely
                // fetch this from a template store/database
                var messageContent = await ResolveTemplate(templateId, templateVars);

                await SendSmsAsync(new SmsMessage
                {
                    To = to,
                    Message = messageContent,
                    From = _settings.FromNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send templated SMS to {To} using template {TemplateId}",
                    to, templateId);
                throw;
            }
        }

        private IDictionary<string, string> ConvertToStringDictionary(object obj)
        {
            var dictionary = new Dictionary<string, string>();
            if (obj == null) return dictionary;

            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj)?.ToString() ?? string.Empty;
                dictionary[prop.Name] = value;
            }

            return dictionary;
        }

        private async Task<string> ResolveTemplate(string templateId, IDictionary<string, string> variables)
        {
            // In a real implementation, you would fetch the template from a store
            // This is a simple example that could be expanded based on needs
            var template = await GetTemplateContent(templateId);

            foreach (var variable in variables)
            {
                template = template.Replace($"{{{variable.Key}}}", variable.Value);
            }

            return template;
        }

        private Task<string> GetTemplateContent(string templateId)
        {
            // In a real implementation, this would fetch from a database or template store
            // For now, we'll throw if template isn't found
            throw new NotImplementedException(
                "Template functionality needs to be implemented with proper template storage");
        }
    }

    public class SmsServiceException : Exception
    {
        public SmsServiceException(string message) : base(message)
        {
        }

        public SmsServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
