using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpeciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Species.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = _context.Species.Find(id);
            if (species == null)
            {
                return NotFound();
            }

            return View(species);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Species species)
        {
            if (ModelState.IsValid)
            {
                _context.Species.Add(species);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(species);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = _context.Species.Find(id);
            if (species == null)
            {
                return NotFound();
            }

            return View(species);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Species species)
        {
            if (id != species.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Species.Update(species);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Species.Any(s => s.Id == species.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(species);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = _context.Species.Find(id);
            if (species == null)
            {
                return NotFound();
            }

            return View(species);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var species = _context.Species.Find(id);
            if (species != null)
            {
                try
                {
                    _context.Species.Remove(species);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "You cannot delete this species while it still has animals in the catalog.");
                    return View(species);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
