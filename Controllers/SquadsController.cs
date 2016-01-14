using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using ColorWars.Models;
using ColorWars.ViewModels.Squad;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace ColorWars.Controllers
{
    public class SquadsController : Controller
    {
        private ApplicationDbContext _context;
        private ClaimsPrincipal CurrentUser => HttpContext.User;

        public SquadsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Squads
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Squads.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Squads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Squad squad = await _context.Squads.SingleAsync(m => m.Id == id);
            if (squad == null)
            {
                return HttpNotFound();
            }

            return View(squad);
        }

        // GET: Squads/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "User");
            return View();
        }

        // POST: Squads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreerSquadViewModel squad)
        {
            if (ModelState.IsValid)
            {
                Squad leSquad = new Squad(squad, CurrentUser.GetUserId());
                // addedSquad.Entity.Id;

                EntityEntry<Squad> addedSquad = _context.Squads.Add(leSquad);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "User", CurrentUser.GetUserId());
            return View(squad);
        }

        // GET: Squads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Squad squad = await _context.Squads.SingleAsync(m => m.Id == id);
            if (squad == null)
            {
                return HttpNotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "User", squad.UserId);
            return View(squad);
        }

        // POST: Squads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Squad squad)
        {
            if (ModelState.IsValid)
            {
                _context.Update(squad);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.AppUser, "Id", "User", squad.UserId);
            return View(squad);
        }

        // GET: Squads/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Squad squad = await _context.Squads.SingleAsync(m => m.Id == id);
            if (squad == null)
            {
                return HttpNotFound();
            }

            return View(squad);
        }

        // POST: Squads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Squad squad = await _context.Squads.SingleAsync(m => m.Id == id);
            _context.Squads.Remove(squad);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
