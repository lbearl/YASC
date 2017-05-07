using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using YASC.Models;
using YASC.Models.CertificateStatusViewModels;
using YASC.Services;

namespace YASC.Controllers
{
    public class VerificationController : Controller
    {
        private readonly ILogger<VerificationController> _logger;
        private readonly ICertificateValidationService _validationService;
        private readonly IMemoryCache _memoryCache;

        public VerificationController(ICertificateValidationService validationService, IMemoryCache memoryCache, ILogger<VerificationController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpGet]
        public IActionResult Index(string url = null)
        {
            return url != null ? View(new CertificateStatusRequestVm { Url = url }) : View();
        }

        [HttpPost]
        public async Task<IActionResult> Result(CertificateStatusRequestVm vm)
        {
            if (ModelState.IsValid)
            {
                if (!_memoryCache.TryGetValue(vm.Url, out CertificateStatus cert))
                {
                    try
                    {
                        var result = await _validationService.VerifyAsync(vm.Url);
                        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));
                        _memoryCache.Set(vm.Url, result, cacheEntryOptions);
                    }
                    catch (Exception e)
                    {
                        _logger.LogDebug(1000, e, $"Failed to get certificate for {vm.Url}", null);
                        ModelState.AddModelError(string.Empty, "Failed to get certificate. Does this domain support SSL?");
                        return View(nameof(Index), vm);
                    }
                }

                return RedirectToAction(nameof(GetResult), new { url = vm.Url });
            }

            return View(nameof(Index), vm);

        }

        [HttpGet]
        public IActionResult GetResult(string url)
        {
            if (_memoryCache.TryGetValue(url, out CertificateStatus cert))
            {
                return View(cert);
            }

            // redirect to the entry page if the cache entry expired.
            return RedirectToAction(nameof(Index), new { url });
        }
    }
}
