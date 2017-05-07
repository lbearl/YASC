using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace YASC.Services
{
    public class DomainAlertEmailSender
    {

        public async Task SendEmailAsync(string emailAddress, string url)
        {
            var apiKey = Environment.GetEnvironmentVariable("SG_YASC_API");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@bearl.me", "No Reply");
            var subject = url;
            var to = new EmailAddress(emailAddress);
            var sgMsg = new SendGridMessage();
            sgMsg.SetFrom(from);
            sgMsg.SetSubject(subject);
            sgMsg.AddTo(to);
            sgMsg.SetTemplateId("e08f6cd0-5b93-429e-9695-18640cb6e78a");
            sgMsg.AddSubstitution("-domain-", url);

            await client.SendEmailAsync(sgMsg);
        }

        public async Task SendNewUserEmailAsync(string emailAddress, string url)
        {
            var apiKey = Environment.GetEnvironmentVariable("SG_YASC_API");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@bearl.me", "No Reply");
            var subject = url;
            var to = new EmailAddress(emailAddress);
            var sgMsg = new SendGridMessage();
            sgMsg.SetFrom(from);
            sgMsg.SetSubject(subject);
            sgMsg.AddTo(to);
            sgMsg.SetTemplateId("6e0214d0-02bd-49a7-91f4-7d00c7661548");
            sgMsg.AddSubstitution("-domain-", url);

            await client.SendEmailAsync(sgMsg);
        }
    }
}
