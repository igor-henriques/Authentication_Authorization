namespace Auth.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<SmtpSetting> smtpSettings;

    public EmailService(IOptions<SmtpSetting> smtpSettings)
    {
        this.smtpSettings = smtpSettings;
    }

    public async Task SendEmailAsync(string from, string to, string subject, string content)
    {
        var mailMessage = new MailMessage(from, to, subject, content);

        using var emailClient = new SmtpClient(smtpSettings.Value.Host, smtpSettings.Value.Port)
        {
            Credentials = new NetworkCredential(smtpSettings.Value.Username, smtpSettings.Value.Password)
        };

        await emailClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailConfirmationAsync(string from, string to, string confirmationLink)
    {
        var mailMessage = new MailMessage(from, to, "Confirme sua conta", 
            $"Confirme sua conta clicando no link: {confirmationLink}");

        using var emailClient = new SmtpClient(smtpSettings.Value.Host, smtpSettings.Value.Port)
        {
            Credentials = new NetworkCredential(smtpSettings.Value.Username, smtpSettings.Value.Password)
        };

        await emailClient.SendMailAsync(mailMessage);
    }
}