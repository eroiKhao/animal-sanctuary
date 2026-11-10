using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var favorites = await _context.Favorites
                .Include(f => f.Animal).ThenInclude(a => a!.Species)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedAt)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int animalId, string? returnUrl = null)
        {
            var userId = _userManager.GetUserId(User);

            var existing = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.AnimalId == animalId);

            if (existing != null)
            {
                _context.Favorites.Remove(existing);
            }
            else
            {
                _context.Favorites.Add(new Favorite
                {
                    UserId = userId!,
                    AnimalId = animalId,
                    AddedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
