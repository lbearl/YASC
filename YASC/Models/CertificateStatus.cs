using System;

namespace YASC.Models
{
    public class CertificateStatus
    {
        public string Url { get; set; }
        public string Issuer { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime NotAfter { get; set; }
        public string Subject { get; set; }
        public string Thumbprint { get; set; }
    }
}
