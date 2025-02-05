

using MSIHORG.Common.Shared.Models.DTOs;

namespace MSIHORG.Common.Server.Core.Services
{
    public interface ISmsService
    {
        Task SendSmsAsync(SmsMessage message);
        Task SendTemplatedSmsAsync(string to, string templateId, object templateData);
    }

}
