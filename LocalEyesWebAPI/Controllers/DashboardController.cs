using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LocalEyesWebAPI.Data;
using LocalEyesWebAPI.Models;
using LocalEyesWebAPI.Services;
using LocalEyesWebAPI.ViewModels;
using Microsoft.AspNetCore.DataProtection;

namespace LocalEyesWebAPI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _pushoverApiDataProtector;
        private readonly IDataProtector _subscriberDataProtector;

        public DashboardController(ILogger<DashboardController> logger, ApplicationDbContext context, IDataProtectionProvider protectionProvider)
        {
            _logger = logger;
            _context = context;
            _pushoverApiDataProtector = protectionProvider.CreateProtector(nameof(PushoverSenderAPIModel));
            _subscriberDataProtector = protectionProvider.CreateProtector(nameof(SubscriberModel));
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel()
            {
                PushoverSenderAPI = await _context.PushoverSenderAPIs.ToListAsync(),
                Subscriber = await _context.Subscribers.ToListAsync()
            };

            // Unprotect api keys
            foreach (var api in viewModel.PushoverSenderAPI)
            {
                api.PushoverSenderAPIKey = _pushoverApiDataProtector.Unprotect(api.PushoverSenderAPIKey);
            }

            foreach (var subscriber in viewModel.Subscriber)
            {
                subscriber.SubscriberPushoverKey = _subscriberDataProtector.Unprotect(subscriber.SubscriberPushoverKey);
            }

            return View(viewModel);
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PushoverSenderAPIs == null)
            {
                return NotFound();
            }

            var pushoverSenderAPI = await _context.PushoverSenderAPIs
                .FirstOrDefaultAsync(m => m.PushoverSenderAPIID == id);
            if (pushoverSenderAPI == null)
            {
                return NotFound();
            }

            // Unprotect api key before view.
            pushoverSenderAPI.PushoverSenderAPIKey = _pushoverApiDataProtector.Unprotect(pushoverSenderAPI.PushoverSenderAPIKey);

            return View(pushoverSenderAPI);
        }

        // GET: Dashboard/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateConfirmAsync([Bind("PushoverSenderAPIID,PushoverSenderName,PushoverSenderAPIKey")] PushoverSenderAPIModel pushoverSenderAPI)
        {
            if (ModelState.IsValid)
            {
                // Protect api key before saving to db.
                pushoverSenderAPI.PushoverSenderAPIKey = _pushoverApiDataProtector.Protect(pushoverSenderAPI.PushoverSenderAPIKey);

                // Add to context.
                _context.Add(pushoverSenderAPI);

                // Try save changes to db.
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(message: "Der opstod en uventet fejl i forsøget på at gemme PushoverApi i databasen: ", ex.Message);

                    // Unprotect key before return view.
                    pushoverSenderAPI.PushoverSenderAPIKey = _pushoverApiDataProtector.Unprotect(pushoverSenderAPI.PushoverSenderAPIKey);
                    return View(pushoverSenderAPI);
                }
                
            }

            return View(pushoverSenderAPI);
        }

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check id and context not null.
            if (id == null || _context.PushoverSenderAPIs == null)
            {
                return NotFound();
            }

            // Load pushover api to edit.
            var pushoverSenderAPI = await _context.PushoverSenderAPIs.FindAsync(id);
            
            // Check pushover api not null.
            if (pushoverSenderAPI == null)
            {
                return NotFound();
            }

            // Unprotect pushover api key before edit.
            pushoverSenderAPI.PushoverSenderAPIKey = _pushoverApiDataProtector.Unprotect(pushoverSenderAPI.PushoverSenderAPIKey);

            // Return view.
            return View(pushoverSenderAPI);
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PushoverSenderAPIID,PushoverSenderName,PushoverSenderAPIKey")] PushoverSenderAPIModel pushoverSenderAPI)
        {
            // Check id match.
            if (id != pushoverSenderAPI.PushoverSenderAPIID)
            {
                return NotFound();
            }

            // Check model state is valid.
            if (ModelState.IsValid)
            {
                // Protect api key before saving to db.
                pushoverSenderAPI.PushoverSenderAPIKey = _pushoverApiDataProtector.Protect(pushoverSenderAPI.PushoverSenderAPIKey);

                // Try save changes.
                try
                {
                    _context.Update(pushoverSenderAPI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Log error and return.
                    if (!PushoverSenderAPIExists(pushoverSenderAPI.PushoverSenderAPIID))
                    {
                        _logger.LogError("Der opstod en uventet fejl i forsøget på at gemme ændringerne i PushoverApiSender: ", ex.Message);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Der opstod en uventet fejl i forsøget på at gemme ændringerne i PushoverApiSender: ", ex.Message);

                        // Unprotect api key before returning view.
                        pushoverSenderAPI.PushoverSenderAPIKey = _pushoverApiDataProtector.Unprotect(pushoverSenderAPI.PushoverSenderAPIKey);

                        return View(pushoverSenderAPI);
                    }
                }

                // save succeded so return to dashboard.
                return RedirectToAction(nameof(Index));
            }

            // Saving failed so return view.
            return View(pushoverSenderAPI);
        }

        // GET: Dashboard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PushoverSenderAPIs == null)
            {
                return NotFound();
            }

            var pushoverSenderAPI = await _context.PushoverSenderAPIs
                .FirstOrDefaultAsync(m => m.PushoverSenderAPIID == id);
            if (pushoverSenderAPI == null)
            {
                return NotFound();
            }

            return View(pushoverSenderAPI);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PushoverSenderAPIs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PushoverSenderAPIs'  is null.");
            }
            var pushoverSenderAPI = await _context.PushoverSenderAPIs.FindAsync(id);
            if (pushoverSenderAPI != null)
            {
                _context.PushoverSenderAPIs.Remove(pushoverSenderAPI);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PushoverSenderAPIExists(int id)
        {
          return _context.PushoverSenderAPIs.Any(e => e.PushoverSenderAPIID == id);
        }
    }
}
