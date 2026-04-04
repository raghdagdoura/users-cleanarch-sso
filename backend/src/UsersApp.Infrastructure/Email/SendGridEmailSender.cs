using System.Net;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using UsersApp.Application.Common.Interfaces;

namespace UsersApp.Infrastructure.Email;

public class SendGridEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SendGridEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(
        string toEmail,
        string subject,
        string htmlContent,
        CancellationToken cancellationToken)
    {
        // 🔑 Lire config
        var apiKey = _configuration["SendGrid:ApiKey"];
        var fromEmail = _configuration["SendGrid:FromEmail"];
        var fromName = _configuration["SendGrid:FromName"];

        if (string.IsNullOrEmpty(apiKey))
            throw new Exception("SendGrid API Key is missing");

        if (string.IsNullOrEmpty(fromEmail))
            throw new Exception("SendGrid FromEmail is missing");

        // 🚀 Créer client
        var client = new SendGridClient(apiKey);

        // 📧 Construire email
        var from = new EmailAddress(fromEmail, fromName);
        var to = new EmailAddress(toEmail);

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            plainTextContent: null,
            htmlContent: htmlContent
        );

        // 📤 Envoi
        var response = await client.SendEmailAsync(msg, cancellationToken);

        var responseBody = await response.Body.ReadAsStringAsync();

        // ❌ Gestion erreur
        if (response.StatusCode != HttpStatusCode.Accepted)
        {
            throw new Exception(
                $"SendGrid failed: {(int)response.StatusCode} - {responseBody}"
            );
        }
    }
}