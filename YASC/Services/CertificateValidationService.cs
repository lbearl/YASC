using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using YASC.Models;

namespace YASC.Services
{
    public class CertificateValidationService : ICertificateValidationService
    {
        public async Task<CertificateStatus> VerifyAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Need a valid URL to verify.");

            if (!url.StartsWith("https://"))
                url = "https://" + url;

            var request = WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            response.Close();

            var cert = (request as HttpWebRequest)?.ServicePoint.Certificate;
            if (cert == null) throw new Exception("Failed to retrieve certificate");

            var c2 = new X509Certificate2(cert);

            return new CertificateStatus
            {
                Url = url,
                Issuer = c2.Issuer,
                NotAfter = c2.NotAfter,
                NotBefore = c2.NotBefore,
                Subject = c2.Subject,
                Thumbprint = c2.Thumbprint
            };
        }

        public static CertificateValidationService Create()
        {
            return new CertificateValidationService();
        }
    }
}
