using System.ComponentModel.DataAnnotations;

namespace YASC.Models.CertificateStatusViewModels
{
    public class CertificateStatusRequestVm
    {
        [Required]
        [RegularExpression(@"(\w+.)?\w+\.\w+", ErrorMessage = "Please ensure that the domain name you entered is just the naked domain i.e. www.example.com or example.com.")]
        public string Url { get; set; }
    }
}
