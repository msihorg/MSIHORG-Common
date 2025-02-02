using Microsoft.Extensions.Logging;
using MSIHORG.Common.Server.Core.Services;
using MSIHORG.Common.Shared.Models.DTOs;
using MSIHORG.Common.Shared.Models.Responses;
using System.Security.Cryptography;

namespace MSIHORG.Common.Server.Infrastructure.Services.Auth
{
    public class PasswordlessAuthService : IPasswordlessAuthService
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        //private readonly ISmsService _smsService;
        //private readonly ITokenService _tokenService;
        //        private readonly ApplicationDbContext _context;
        private readonly ILogger<PasswordlessAuthService> _logger;
        // private readonly IConfiguration _configuration;

        public PasswordlessAuthService(

            IEmailService emailService,
            ILogger<PasswordlessAuthService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        //    public PasswordlessAuthService(
        //UserManager<ApplicationUser> userManager,
        //IEmailService emailService,
        //ISmsService smsService,
        //ITokenService tokenService,
        //ApplicationDbContext context,
        //ILogger<PasswordlessAuthService> logger,
        //IConfiguration configuration)
        //    {
        //        _userManager = userManager;
        //        _emailService = emailService;
        //        _smsService = smsService;
        //        _tokenService = tokenService;
        //        _context = context;
        //        _logger = logger;
        //        _configuration = configuration;
        //    }

        public async Task<Result<string>> InitiateLoginAsync(string contact, string contactType)
        {
            try
            {
                // Generate secure random token
                var token = GenerateSecureToken();

                // Generate magic link
                var baseUrl = ""; //_configuration["AppSettings:BaseUrl"];
                var magicLink = $"{baseUrl}/auth/verify?token={token}";

                // Send link via email or SMS
                if (contactType.ToLower() == "email")
                {
                    await _emailService.SendEmailAsync(new EmailMessage
                    {
                        To = contact,
                        Subject = "Your Login Link",
                        Body = $"Click here to log in: {magicLink}",
                        IsHtml = true
                    });
                }

                return Result<string>.Success("Login link sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating passwordless login for {Contact}", contact);
                return Result<string>.Failure("Error processing login request");
            }
        }

        public async Task<Result<string>> ValidateLoginTokenAsync(string token)
        {
            try
            {
                return Result<string>.Success("Success processing login verification");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating login token");
                return Result<string>.Failure("Error processing login verification");
            }
        }

        private static string GenerateSecureToken(int length = 12)
        {
            int numBytes = (int)Math.Ceiling(length * 0.75);

            var randomBytes = new byte[numBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string result = Convert.ToBase64String(randomBytes)
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "");

            return result.Substring(0, Math.Min(length, result.Length));
        }
    }
}
