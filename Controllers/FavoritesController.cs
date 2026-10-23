using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public const string VisitorKeySession = "VisitorKey";

        private string GetOrCreateVisitorKey()
        {
            var key = HttpContext.Session.GetString(VisitorKeySession);

            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString("N");
                HttpContext.Session.SetString(VisitorKeySession, key);
            }

            return key;
        }

        public IActionResult Index()
        {
            var key = GetOrCreateVisitorKey();

            var favorites = _context.Favorites
                .Include(f => f.Animal)
                    .ThenInclude(a => a!.Species)
                .Where(f => f.VisitorKey == key)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();

            return View(favorites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int animalId, string? returnUrl)
        {
            var animal = _context.Animals.Find(animalId);
            if (animal == null)
            {
                return NotFound();
            }

            var key = GetOrCreateVisitorKey();
            var existing = _context.Favorites
                .FirstOrDefault(f => f.AnimalId == animalId && f.VisitorKey == key);

            if (existing == null)
            {
                _context.Favorites.Add(new Favorite
                {
                    AnimalId = animalId,
                    VisitorKey = key,
                    CreatedAt = DateTime.Now
                });
                TempData["FavoriteMessage"] = $"{animal.Name} was added to your favourites.";
            }
            else
            {
                _context.Favorites.Remove(existing);
                TempData["FavoriteMessage"] = $"{animal.Name} was removed from your favourites.";
            }

            _context.SaveChanges();

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction(nameof(Index));
            }

            return Redirect(returnUrl);
        }
    }
}
