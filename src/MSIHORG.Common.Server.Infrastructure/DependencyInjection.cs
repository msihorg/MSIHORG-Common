using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSIHORG.Common.Server.Core.Services;
using MSIHORG.Common.Server.Infrastructure.Services.Email;
using MSIHORG.Common.Server.Infrastructure.Services.Payments;
using MSIHORG.Common.Server.Infrastructure.Services.Settings.SMS;
using MSIHORG.Common.Server.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure and validate service settings based on providers
            var emailProvider = configuration["EmailProvider"]?.ToLower();
            if (emailProvider == "sendgrid")
            {
                services.Configure<SendGridSettings>(options =>
                    configuration.GetSection("SendGridSettings").Bind(options));
                var sendGridSettings = configuration
                    .GetSection("SendGridSettings")
                    .Get<SendGridSettings>();
                sendGridSettings?.Validate();
            }

            var smsProvider = configuration["SmsProvider"]?.ToLower();
            if (smsProvider == "twilio")
            {
                services.Configure<TwilioSettings>(options =>
                    configuration.GetSection("TwilioSettings").Bind(options));
                var twilioSettings = configuration
                    .GetSection("TwilioSettings")
                    .Get<TwilioSettings>();
                twilioSettings?.Validate();
            }

            var paymentProvider = configuration["PaymentProvider"]?.ToLower();
            if (paymentProvider == "braintree")
            {
                services.Configure<BraintreeSettings>(options =>
                    configuration.GetSection("BraintreeSettings").Bind(options));
                var braintreeSettings = configuration
                    .GetSection("BraintreeSettings")
                    .Get<BraintreeSettings>();
                braintreeSettings?.Validate();
            }

            // Register services based on configuration
            switch (emailProvider)
            {
                case "sendgrid":
                    services.AddScoped<IEmailService, SendGridEmailService>();
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported email provider: {emailProvider}");
            }

            switch (smsProvider)
            {
                case "twilio":
                    services.AddScoped<ISmsService, TwilioSmsService>();
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported SMS provider: {smsProvider}");
            }

            switch (paymentProvider)
            {
                case "braintree":
                    services.AddScoped<IPaymentService, BraintreePaymentService>();
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Unsupported payment provider: {paymentProvider}");
            }

            return services;
        }
    }
}
