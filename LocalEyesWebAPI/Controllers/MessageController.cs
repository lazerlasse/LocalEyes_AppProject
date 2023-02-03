using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LocalEyesWebAPI.Data;
using LocalEyesWebAPI.Models;

namespace LocalEyesWebAPI.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MessageController> _logger;

        public MessageController(ApplicationDbContext context, ILogger<MessageController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: MessageModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.ToListAsync());
        }

        // GET: MessageModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var messageModel = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageID == id);
            if (messageModel == null)
            {
                return NotFound();
            }

            return View(messageModel);
        }

        // GET: MessageModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Messages == null)
            {
                return NotFound();
            }

            var messageToDelete = await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageID == id);

            if (messageToDelete == null)
            {
                return NotFound();
            }

            return View(messageToDelete);
        }

        // POST: MessageModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Messages == null)
            {
                return Problem("Beskeder kunne ikke indlæses fra databasen!");
            }

            var messageToDelete = await _context.Messages.FindAsync(id);

            if (messageToDelete == null)
            {
                return NotFound();
            }

            try
            {
                FileHandlerService.DeleteMediaFolderAndFiles(id);
                _context.Messages.Remove(messageToDelete);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Der opstod en uventet fejl i forsøget på at slette beskeden og tilhørende filer", ex.Message);
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Beskeden og tilhørende filer blev slettet med succes.");
            return RedirectToAction(nameof(Index));
        }

        private bool MessageModelExists(int id)
        {
            return _context.Messages.Any(e => e.MessageID == id);
        }
    }
}
