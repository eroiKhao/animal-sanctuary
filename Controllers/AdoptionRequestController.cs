using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    public class AdoptionRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdoptionRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var requests = _context.AdoptionRequests
                .Include(r => r.Animal)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(requests);
        }

        public IActionResult Create(int animalId)
        {
            var animal = _context.Animals
                .Include(a => a.Species)
                .FirstOrDefault(a => a.Id == animalId);

            if (animal == null)
            {
                return NotFound();
            }

            ViewBag.Animal = animal;

            var request = new AdoptionRequest
            {
                AnimalId = animal.Id,
                RequestType = "Adoption",
                PreferredVisitDate = DateTime.Today.AddDays(1)
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdoptionRequest request)
        {
            var animal = _context.Animals.Find(request.AnimalId);
            if (animal == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Animal = _context.Animals
                    .Include(a => a.Species)
                    .First(a => a.Id == request.AnimalId);
                return View(request);
            }

            request.Status = "Pending";
            request.CreatedAt = DateTime.Now;

            _context.AdoptionRequests.Add(request);
            _context.SaveChanges();

            TempData["RequestSuccess"] =
                $"Your application for {animal.Name} has been submitted! Our manager will contact you soon.";

            return RedirectToAction(nameof(Index));
        }
    }
}
