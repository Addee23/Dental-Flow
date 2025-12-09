using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        Console.WriteLine("=== SKICKAR EMAIL ===");

        var host = _config["Smtp:Host"];
        var port = int.Parse(_config["Smtp:Port"]);
        var user = _config["Smtp:User"];
        var password = _config["Smtp:Password"];
        var enableSsl = bool.Parse(_config["Smtp:EnableSsl"]);

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, password),
            EnableSsl = enableSsl
        };

        var mail = new MailMessage
        {
            From = new MailAddress(user, "DentalFlow"),
            Subject = subject,
            Body = message
        };

        mail.To.Add(toEmail);

        await client.SendMailAsync(mail);
    }


}

