using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YASC.Data;
using YASC.Models;
using Microsoft.AspNetCore.Authorization;
using YASC.Services;

namespace YASC.Controllers
{
    [Authorize]
    public class RecurringAlertController : Controller
    {
        private readonly IAlertingService _alertingService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecurringAlertController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IAlertingService alertingService)
        {
            _alertingService = alertingService ?? throw new ArgumentNullException(nameof(alertingService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: RecurringAlerts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecurringAlerts.Include(r => r.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecurringAlerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recurringAlert = await _context.RecurringAlerts
                .Include(r => r.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recurringAlert == null)
            {
                return NotFound();
            }

            return View(recurringAlert);
        }

        // GET: RecurringAlerts/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: RecurringAlerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,EmailAddress,Url")] RecurringAlert recurringAlert)
        {
            if (ModelState.IsValid)
            {
                recurringAlert.CreatedOn = DateTime.Now;
                recurringAlert.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
                _context.Add(recurringAlert);
                await _context.SaveChangesAsync();
                _alertingService.AddJob(recurringAlert.Url, recurringAlert.EmailAddress);
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id",
                recurringAlert.ApplicationUserId);
            return View(recurringAlert);
        }

        // GET: RecurringAlerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recurringAlert = await _context.RecurringAlerts.SingleOrDefaultAsync(m => m.Id == id);
            if (recurringAlert == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id",
                recurringAlert.ApplicationUserId);
            return View(recurringAlert);
        }

        // POST: RecurringAlerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,EmailAddress,CreatedOn,Url,ApplicationUserId")] RecurringAlert recurringAlert)
        {
            if (id != recurringAlert.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recurringAlert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecurringAlertExists(recurringAlert.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id",
                recurringAlert.ApplicationUserId);
            return View(recurringAlert);
        }

        // GET: RecurringAlerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recurringAlert = await _context.RecurringAlerts
                .Include(r => r.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recurringAlert == null)
            {
                return NotFound();
            }

            return View(recurringAlert);
        }

        // POST: RecurringAlerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recurringAlert = await _context.RecurringAlerts.SingleOrDefaultAsync(m => m.Id == id);
            _context.RecurringAlerts.Remove(recurringAlert);
            await _context.SaveChangesAsync();
            RecurringJob.RemoveIfExists(recurringAlert.Url);
            return RedirectToAction("Index");
        }

        private bool RecurringAlertExists(int id)
        {
            return _context.RecurringAlerts.Any(e => e.Id == id);
        }
    }
}