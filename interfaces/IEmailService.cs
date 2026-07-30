namespace LogisticsERP.API.interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task SendPasswordResetEmailAsync(string toEmail, string fullName, string resetLink);
        Task SendAccountApprovedEmailAsync(string toEmail, string fullName);
        Task SendAccountRejectedEmailAsync(string toEmail, string fullName, string? reason);

    }
}
