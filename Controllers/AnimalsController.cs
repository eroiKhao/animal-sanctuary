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

        public IActionResult Index()
        {
            var animals = _context.Animals
                .Include(a => a.Species)
                .ToList();

            return View(animals);
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
