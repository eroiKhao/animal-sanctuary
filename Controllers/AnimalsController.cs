using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, int? speciesFilter,
            string genderFilter, string healthFilter, string sortOrder)
        {
            ViewData["SearchString"] = searchString;
            ViewData["SpeciesFilter"] = speciesFilter;
            ViewData["GenderFilter"] = genderFilter;
            ViewData["HealthFilter"] = healthFilter;

            var animals = _context.Animals.Include(a => a.Species).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                animals = animals.Where(a => a.Name.Contains(searchString)
                                          || a.Description.Contains(searchString));
            }

            if (speciesFilter.HasValue && speciesFilter.Value > 0)
            {
                animals = animals.Where(a => a.SpeciesId == speciesFilter.Value);
            }

            if (!string.IsNullOrEmpty(genderFilter))
            {
                animals = animals.Where(a => a.Gender == genderFilter);
            }

            if (!string.IsNullOrEmpty(healthFilter))
            {
                animals = animals.Where(a => a.HealthStatus == healthFilter);
            }

            switch (sortOrder)
            {
                case "name":
                    animals = animals.OrderBy(a => a.Name);
                    break;
                case "age":
                    animals = animals.OrderBy(a => a.AgeMonths);
                    break;
                default:
                    animals = animals.OrderByDescending(a => a.CreatedAt);
                    break;
            }

            ViewBag.SpeciesList = new SelectList(_context.Species, "Id", "Name", speciesFilter);

            var visitorKey = HttpContext.Session.GetString(FavoritesController.VisitorKeySession);
            ViewBag.FavoriteIds = visitorKey == null
                ? new HashSet<int>()
                : _context.Favorites
                    .Where(f => f.VisitorKey == visitorKey)
                    .Select(f => f.AnimalId)
                    .ToHashSet();

            return View(animals.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = _context.Animals
                .Include(a => a.Species)
                .FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            var visitorKey = HttpContext.Session.GetString(FavoritesController.VisitorKeySession);
            ViewBag.IsFavorite = visitorKey != null &&
                _context.Favorites.Any(f => f.VisitorKey == visitorKey && f.AnimalId == animal.Id);

            return View(animal);
        }

        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Animal animal)
        {
            if (animal.SpeciesId == 0)
            {
                ModelState.AddModelError("SpeciesId", "Please select a species.");
            }

            if (ModelState.IsValid)
            {
                animal.CreatedAt = DateTime.Now;
                _context.Animals.Add(animal);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists();
            return View(animal);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = _context.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            PopulateDropDownLists();
            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (animal.SpeciesId == 0)
            {
                ModelState.AddModelError("SpeciesId", "Please select a species.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Animals.Update(animal);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Animals.Any(a => a.Id == animal.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists();
            return View(animal);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = _context.Animals
                .Include(a => a.Species)
                .FirstOrDefault(a => a.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var animal = _context.Animals.Find(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDownLists()
        {
            ViewBag.SpeciesId = new SelectList(_context.Species, "Id", "Name");
            ViewBag.Gender = new SelectList(new[] { "Male", "Female" });
            ViewBag.HealthStatus = new SelectList(new[] { "Healthy", "Under Treatment", "Recovered" });
            ViewBag.Status = new SelectList(new[] { "Available", "Reserved", "Adopted" });
        }
    }
}
