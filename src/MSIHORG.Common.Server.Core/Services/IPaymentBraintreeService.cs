using Braintree;

namespace MSIHORG.Common.Server.Core.Services
{
    public interface IPaymentBraintreeService
    {
        IBraintreeGateway CreateGateway();
        string GetConfigurationSetting(string setting);
        IBraintreeGateway GetGateway();
    }
}
