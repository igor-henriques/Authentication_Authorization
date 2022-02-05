namespace Auth.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string from, string to, string subject, string content);
    Task SendEmailConfirmationAsync(string from, string to, string confirmationLink);
}