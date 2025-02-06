using MSIHORG.Common.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Core.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task<string> GenerateClientTokenAsync();
        Task<bool> ValidatePaymentMethodAsync(string paymentMethodToken);
    }
}

