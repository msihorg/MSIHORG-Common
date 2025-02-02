// BlazorTemplate.API\Controllers\AuthController.cs
using BlazorTemplate.Core.Interfaces.Services;
using BlazorTemplate.Shared.Web.Common;
using BlazorTemplate.Shared.Web.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MSIHORG.Common.Server.Webapi.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IPasswordlessAuthService _passwordlessAuthService;

        public AuthController(IPasswordlessAuthService passwordlessAuthService)
        {
            _passwordlessAuthService = passwordlessAuthService;
        }

        [HttpPost("login/passwordless")]
        public async Task<ActionResult<string>> InitiatePasswordlessLogin(
            PasswordlessLoginRequest request)
        {
            var result = await _passwordlessAuthService.InitiateLoginAsync(
                request.Contact, request.ContactType);
            return HandleResult(result);
        }

        [HttpGet("verify")]
        public async Task<ActionResult<AuthResponse>> VerifyLoginToken(
            [FromQuery] string token)
        {
            var result = await _passwordlessAuthService.ValidateLoginTokenAsync(token);
            return HandleResult(result);
        }

        private ActionResult<T> HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
