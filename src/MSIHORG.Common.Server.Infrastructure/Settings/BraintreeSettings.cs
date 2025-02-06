using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Infrastructure.Settings
{
    public class BraintreeSettings
    {
        public const string SectionName = "BraintreeSettings";

        /// <summary>
        /// Braintree Environment (sandbox/production)
        /// </summary>
        public string Environment { get; set; } = "sandbox";

        /// <summary>
        /// Your Braintree Merchant ID
        /// </summary>
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// Your Braintree Public Key
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// Your Braintree Private Key
        /// </summary>
        public string PrivateKey { get; set; } = string.Empty;

        /// <summary>
        /// Optional: Enable 3D Secure payments
        /// </summary>
        public bool Enable3DSecure { get; set; }

        /// <summary>
        /// Optional: Enable PayPal integration
        /// </summary>
        public bool EnablePayPal { get; set; }

        /// <summary>
        /// Optional: Enable detailed logging for debugging
        /// </summary>
        public bool EnableDetailedLogging { get; set; }

        /// <summary>
        /// Validate required settings
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(MerchantId))
                throw new InvalidOperationException($"{nameof(MerchantId)} is required");

            if (string.IsNullOrWhiteSpace(PublicKey))
                throw new InvalidOperationException($"{nameof(PublicKey)} is required");

            if (string.IsNullOrWhiteSpace(PrivateKey))
                throw new InvalidOperationException($"{nameof(PrivateKey)} is required");

            if (string.IsNullOrWhiteSpace(Environment) ||
                (Environment.ToLower() != "sandbox" && Environment.ToLower() != "production"))
                throw new InvalidOperationException($"{nameof(Environment)} must be either 'sandbox' or 'production'");
        }
    }
}
