using Braintree;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSIHORG.Common.Server.Core.Services;
using MSIHORG.Common.Server.Infrastructure.Settings;
using MSIHORG.Common.Shared.Models.DTOs;
using MSIHORG.Common.Shared.Models.Responses;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;

namespace MSIHORG.Common.Server.Infrastructure.Services.Payments
{
    public class BraintreePaymentService : IPaymentService
    {
        private readonly IBraintreeGateway _gateway;
        private readonly ILogger<BraintreePaymentService> _logger;

        public BraintreePaymentService(
            IOptions<BraintreeSettings> settings,
            ILogger<BraintreePaymentService> logger)
        {
            _logger = logger;
            _gateway = new BraintreeGateway
            {
                Environment = settings.Value.Environment == "sandbox"
                    ? Braintree.Environment.SANDBOX
                    : Braintree.Environment.PRODUCTION,
                MerchantId = settings.Value.MerchantId,
                PublicKey = settings.Value.PublicKey,
                PrivateKey = settings.Value.PrivateKey
            };
        }

        public async Task<string> GenerateClientTokenAsync()
        {
            try
            {
                var clientToken = await _gateway.ClientToken.GenerateAsync();
                if (string.IsNullOrEmpty(clientToken))
                {
                    throw new InvalidOperationException("Failed to generate client token");
                }
                return clientToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate client token");
                throw new PaymentServiceException("Failed to generate client token", ex);
            }
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                var transactionRequest = new TransactionRequest
                {
                    Amount = request.Amount,
                    PaymentMethodNonce = request.PaymentMethodNonce,
                    CustomerId = request.CustomerId,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true,
                        StoreInVaultOnSuccess = true
                    },
                    CustomFields = new Dictionary<string, string>()
                };

                // Add custom fields from metadata
                foreach (var item in request.Metadata)
                {
                    transactionRequest.CustomFields.Add(item.Key, item.Value);
                }

                Braintree.Result<Transaction> result = await _gateway.Transaction.SaleAsync(transactionRequest);

                if (result.IsSuccess())
                {
                    return new PaymentResult
                    {
                        Success = true,
                        TransactionId = result.Target.Id,
                        Status = result.Target.Status.ToString(),
                        ErrorMessage = null
                    };
                }

                return new PaymentResult
                {
                    Success = false,
                    TransactionId = string.Empty,
                    Status = "Failed",
                    ErrorMessage = string.Join(", ", result.Errors.All().Select(e => e.Message))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process payment for amount {Amount}", request.Amount);
                throw new PaymentServiceException("Failed to process payment", ex);
            }
        }

        public async Task<bool> ValidatePaymentMethodAsync(string paymentMethodToken)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentMethodToken))
                {
                    return false;
                }

                var paymentMethod = await _gateway.PaymentMethod.FindAsync(paymentMethodToken);
                if (paymentMethod == null)
                {
                    return false;
                }

                // Additional validation can be added here based on payment method status
                // Assuming we need to check if the payment method is default instead of revoked
                return paymentMethod.IsDefault.HasValue && paymentMethod.IsDefault.Value;
            }
            catch (NotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to validate payment method {Token}", paymentMethodToken);
                throw new PaymentServiceException("Failed to validate payment method", ex);
            }
        }
    }

    public class PaymentServiceException : Exception
    {
        public PaymentServiceException(string message) : base(message) { }
        public PaymentServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

