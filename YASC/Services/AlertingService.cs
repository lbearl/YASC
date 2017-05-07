using System;
using System.Threading.Tasks;
using Hangfire;

namespace YASC.Services
{
    public class AlertingService : IAlertingService
    {
        public void AddJob(string url, string emailAddress)
        {
            RecurringJob.AddOrUpdate(url, () => AlertingService.CheckDomain(url, emailAddress), Cron.Daily());
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static async Task CheckDomain(string url, string emailAddress)
        {
            var result = await CertificateValidationService.Create().VerifyAsync(url);
            if(result.NotAfter < DateTime.Now.AddDays(-30))
            {
                await new DomainAlertEmailSender().SendEmailAsync(emailAddress, url);
            }
        }
    }
}
