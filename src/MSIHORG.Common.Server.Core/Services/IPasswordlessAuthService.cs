using MSIHORG.Common.Shared.Models.DTOs;
using MSIHORG.Common.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIHORG.Common.Server.Core.Services
{
    public interface IPasswordlessAuthService
    {
        Task<Result<string>> InitiateLoginAsync(string contact, string contactType);
        Task<Result<string>> ValidateLoginTokenAsync(string token);
    }
}
