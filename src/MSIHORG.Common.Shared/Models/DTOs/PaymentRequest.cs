namespace MSIHORG.Common.Shared.Models.DTOs
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string PaymentMethodNonce { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public IDictionary<string, string> Metadata { get; set; } =
            new Dictionary<string, string>();
    }
}

