using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnimalSanctuary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalAnimals = _context.Animals.Count();
            ViewBag.AvailableAnimals = _context.Animals.Count(a => a.Status == "Available");
            ViewBag.AdoptedAnimals = _context.Animals.Count(a => a.Status == "Adopted");
            ViewBag.SpeciesCount = _context.Species.Count();

            return View();
        }
        public IActionResult SupportUs()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
