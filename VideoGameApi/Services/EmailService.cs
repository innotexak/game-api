using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using VideoGameApi.Model;
using VideoGameApi.Services.Interfaces;

namespace VideoGameApi.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public async Task SendEmail(IEmailDto payload)
        {

            var email = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email, payload.To, payload.Subject, payload.Body);

            await smtpClient.SendMailAsync(message);
        }
    }
}
