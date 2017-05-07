using System.Threading.Tasks;
using YASC.Models;

namespace YASC.Services
{
    public interface ICertificateValidationService
    {
        Task<CertificateStatus> VerifyAsync(string url);
    }
}
