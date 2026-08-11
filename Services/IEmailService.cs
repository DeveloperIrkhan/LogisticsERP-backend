using LogisticsERP.API.DTOs.Auth;
using LogisticsERP.API.interfaces;
using System.Net;
using System.Net.Mail;

namespace LogisticsERP.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task SendPasswordResetEmailAsync(string toEmail, string fullName, string resetLink)
        {
            var body = $"""
                <div style="font-family: Arial, sans-serif; max-width: 560px; margin: auto;">
                    <h2 style="color:#dc2626;">Password Reset Request</h2>
                    <p>Hi {fullName},</p>
                    <p>We received a request to reset your password for your PRCS Logistics ERP account.</p>
                    <p>
                        <a href="{resetLink}"
                           style="background:#dc2626;color:#fff;padding:12px 24px;border-radius:8px;text-decoration:none;display:inline-block;">
                           Reset Password
                        </a>
                    </p>
                    <p>This link expires in 1 hour. If you did not request this, you can safely ignore this email.</p>
                </div>
                """;

            return SendEmailAsync(toEmail, "Reset your PRCS Logistics ERP password", body);
        }

        public Task SendAccountApprovedEmailAsync(string toEmail, string fullName)
        {
            var body = $"""
                <div style="font-family: Arial, sans-serif; max-width: 560px; margin: auto;">
                    <h2 style="color:#16a34a;">Account Approved</h2>
                    <p>Hi {fullName},</p>
                    <p>Your PRCS Logistics ERP account has been approved. You can now log in.</p>
                </div>
                """;

            return SendEmailAsync(toEmail, "Your PRCS Logistics ERP account is approved", body);
        }

        public Task SendAccountRejectedEmailAsync(string toEmail, string fullName, string? reason)
        {
            var reasonHtml = string.IsNullOrWhiteSpace(reason) ? "" : $"<p>Reason: {reason}</p>";
            var body = $"""
                <div style="font-family: Arial, sans-serif; max-width: 560px; margin: auto;">
                    <h2 style="color:#dc2626;">Account Request Declined</h2>
                    <p>Hi {fullName},</p>
                    <p>Your PRCS Logistics ERP account request was not approved.</p>
                    {reasonHtml}
                </div>
                """;

            return SendEmailAsync(toEmail, "Your PRCS Logistics ERP account request", body);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var host = _config["EmailSettings:Host"];
            var port = int.TryParse(_config["EmailSettings:Port"], out var p) ? p : 587;
            var enableSsl = bool.TryParse(_config["EmailSettings:EnableSsl"], out var ssl) ? ssl : true;
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderPassword = _config["EmailSettings:SenderPassword"];
            var senderName = _config["EmailSettings:SenderName"] ?? "Logistics ERP";

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(senderEmail))
            {
                // Don't crash the request flow (e.g. registration) just because email isn't configured yet.
                _logger.LogWarning("EmailSettings not configured — skipped sending email to {ToEmail} with subject '{Subject}'.", toEmail, subject);
                return;
            }

            using var message = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = enableSsl,
            };

            await client.SendMailAsync(message);
        }

        public async Task SendDriverPasswordLink(string toEmail, string subject, string text, RegisterDto? dto)
        {
            var host = _config["EmailSettings:Host"];
            var port = int.TryParse(_config["EmailSettings:Port"], out var p) ? p : 587;
            var enableSsl = bool.TryParse(_config["EmailSettings:EnableSsl"], out var ssl) ? ssl : true;
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderPassword = _config["EmailSettings:SenderPassword"];
            var senderName = _config["EmailSettings:SenderName"] ?? "Logistics ERP";

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(senderEmail))
            {
                // Don't crash the request flow (e.g. registration) just because email isn't configured yet.
                _logger.LogWarning("EmailSettings not configured — skipped sending email to {ToEmail} with subject '{Subject}'.", toEmail, subject);
                return;
            }
            var frontendBaseUrl = _config["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:3000";
            var resetLink = $"{frontendBaseUrl}/auth/login";
            var body = $"""
                <div style="font-family: Arial, sans-serif; max-width: 560px; margin: auto;">
                    <h2 style="color:#dc2626;">Password Reset Request</h2>
                    <p>Hi, Mr <span style="text-bold; color="#dc3426">{dto?.Fullname ?? "User"}</span></p>
                    <p>
                        {text}
                    </p>
                <h2 style="color:#dc2626;">Login Page</h2>
                 <p>
                        <a href="{resetLink}"
                           style="background:#dc2626;color:#fff;padding:12px 24px;border-radius:8px;text-decoration:none;display:inline-block;">
                          Login here
                        </a>
                    </p>
                </div>
                """;
            using var message = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = enableSsl,
            };

            await client.SendMailAsync(message);
        }

    }
}
