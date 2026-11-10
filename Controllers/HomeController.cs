using System.Diagnostics;
using AnimalSanctuary.Data;
using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index(string searchString, int? speciesFilter,
            string genderFilter, string healthFilter, string sortOrder)
        {
            ViewBag.TotalAnimals = _context.Animals.Count();
            ViewBag.AvailableAnimals = _context.Animals.Count(a => a.Status == "Available");
            ViewBag.AdoptedAnimals = _context.Animals.Count(a => a.Status == "Adopted");
            ViewBag.SpeciesCount = _context.Species.Count();

            ViewData["SearchString"] = searchString;
            ViewData["SpeciesFilter"] = speciesFilter;
            ViewData["GenderFilter"] = genderFilter;
            ViewData["HealthFilter"] = healthFilter;
            ViewData["SortOrder"] = sortOrder;

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

            return View(animals.ToList());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult SupportUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
