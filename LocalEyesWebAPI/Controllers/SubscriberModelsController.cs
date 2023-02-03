using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LocalEyesWebAPI.Data;
using LocalEyesWebAPI.Models;
using Microsoft.AspNetCore.DataProtection;

namespace LocalEyesWebAPI.Controllers
{
    public class SubscriberModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubscriberModelsController> _logger;
        private readonly IDataProtector _dataProtector;

        public SubscriberModelsController(
            ApplicationDbContext context,
            ILogger<SubscriberModelsController> logger,
            IDataProtectionProvider protectionProvider)
        {
            _context = context;
            _logger = logger;
            _dataProtector = protectionProvider.CreateProtector(nameof(SubscriberModel));
        }

        // GET: SubscriberModels/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubscriberModels/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm([Bind("SubscriberModelID,SubscriberName,SubscriberPushoverKey,SubscriberEnabled")] SubscriberModel subscriberModel)
        {
            if (ModelState.IsValid)
            {
                // Protect pushover key.
                subscriberModel.SubscriberPushoverKey = _dataProtector.Protect(subscriberModel.SubscriberPushoverKey);
                
                // Add new subscriber to db context.
                _context.Add(subscriberModel);

                // Try save changes to db.
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Log error and return to view.
                    _logger.LogError("Der opstod en uventet fejl i forsøget på at oprette subscriber: ", ex.Message);

                    // Unprotect pushover key before returning to view.
                    subscriberModel.SubscriberPushoverKey = _dataProtector.Unprotect(subscriberModel.SubscriberPushoverKey);
                    
                    // Return view.
                    return View(subscriberModel);
                }
                
                // Log succes message and return to dashbord.
                _logger.LogInformation("Besked modtageren blev oprettet med succes: ", subscriberModel.SubscriberName);
                return RedirectToAction(nameof(Index), "Dashboard");
            }

            // Model state not valid return to view.
            return View(subscriberModel);
        }

        // GET: SubscriberModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Check id and context not null.
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            // Try to load subscriber from db.
            var subscriberModel = await _context.Subscribers.FindAsync(id);
            
            // Check subscriber is loaded and not null.
            if (subscriberModel == null)
            {
                return NotFound();
            }

            // Unprotect pushover key before edit.
            subscriberModel.SubscriberPushoverKey = _dataProtector.Unprotect(subscriberModel.SubscriberPushoverKey);
            
            // Return edit view.
            return View(subscriberModel);
        }

        // POST: SubscriberModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubscriberModelID,SubscriberName,SubscriberPushoverKey,SubscriberEnabled")] SubscriberModel subscriberModel)
        {
            // Check id and subscriber not null.
            if (id != subscriberModel.SubscriberModelID)
            {
                return NotFound();
            }

            // Check model state validity
            if (ModelState.IsValid)
            {
                // Protect pushover key before saving to db.
                subscriberModel.SubscriberPushoverKey = _dataProtector.Protect(subscriberModel.SubscriberPushoverKey);

                // Try save changes to db.
                try
                {
                    _context.Update(subscriberModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SubscriberModelExists(subscriberModel.SubscriberModelID))
                    {
                        _logger.LogError($"Der opstod en uventet fejl i forsøget på at opdatere besked modtageren, da denne ikke blev fundet: {subscriberModel.SubscriberName} - ", ex.Message);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Der opstod en uventet fejl i forsøget på at opdatere besked modtageren: {subscriberModel.SubscriberName} - ", ex.Message);

                        // Unprotect pushover key before returning to view.
                        subscriberModel.SubscriberPushoverKey = _dataProtector.Unprotect(subscriberModel.SubscriberPushoverKey);
                        
                        // Return to view.
                        return View(subscriberModel);
                    }
                }

                // Log info and return to dashboard.
                _logger.LogInformation($"Besked modtageren {subscriberModel.SubscriberName} blev opdateret med succes.");
                return RedirectToAction(nameof(Index), "Dashboard");
            }

            // Saving changes failed return view.
            return View(subscriberModel);
        }

        // GET: SubscriberModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscriberModel = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.SubscriberModelID == id);

            if (subscriberModel == null)
            {
                return NotFound();
            }

            return View(subscriberModel);
        }

        // POST: SubscriberModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscribers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subscribers'  is null.");
            }

            var subscriberModel = await _context.Subscribers.FindAsync(id);

            if (subscriberModel != null)
            {
                _context.Subscribers.Remove(subscriberModel);
            }

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Besked modtageren blev slettet med succes: ", subscriberModel.SubscriberName);
            }
            catch (Exception ex)
            {
                // Log exception.
                _logger.LogError("Der opstod en uventet fejl i forsøget på at slette besked modtageren: ", ex.Message);
            }
            
            // Return to dashboard.
            return RedirectToAction(nameof(Index), "Dashboard");
        }

        private bool SubscriberModelExists(int id)
        {
            return _context.Subscribers.Any(e => e.SubscriberModelID == id);
        }
    }
}
