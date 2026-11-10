using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    public class AdoptionRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdoptionRequestsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(int animalId)
        {
            var animal = await _context.Animals
                .Include(a => a.Species)
                .FirstOrDefaultAsync(a => a.Id == animalId);

            if (animal == null)
            {
                return NotFound();
            }

            if (animal.Status != "Available")
            {
                TempData["RequestError"] =
                    $"Sorry, applications for {animal.Name} are closed - the animal is already {animal.Status.ToLower()}.";
                return RedirectToAction("Details", "Animals", new { id = animalId });
            }

            var user = await _userManager.GetUserAsync(User);
            var model = new AdoptionRequest
            {
                AnimalId = animal.Id,
                PhoneNumber = user?.PhoneNumber ?? string.Empty
            };

            ViewBag.Animal = animal;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdoptionRequest model)
        {
            var animal = await _context.Animals.FindAsync(model.AnimalId);

            if (animal == null)
            {
                return NotFound();
            }

            if (animal.Status != "Available")
            {
                TempData["RequestError"] =
                    $"Sorry, applications for {animal.Name} are closed - the animal is already {animal.Status.ToLower()}.";
                return RedirectToAction("Details", "Animals", new { id = animal.Id });
            }

            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                ViewBag.Animal = await _context.Animals
                    .Include(a => a.Species)
                    .FirstAsync(a => a.Id == model.AnimalId);
                return View(model);
            }

            model.UserId = _userManager.GetUserId(User)!;
            model.Status = "Pending";
            model.CreatedAt = DateTime.Now;

            _context.AdoptionRequests.Add(model);
            await _context.SaveChangesAsync();

            TempData["RequestSuccess"] =
                $"Your application for {animal.Name} has been submitted! Our manager will contact you soon.";

            return RedirectToAction(nameof(MyApplications));
        }


        [Authorize]
        public async Task<IActionResult> MyApplications()
        {
            var userId = _userManager.GetUserId(User);

            var requests = await _context.AdoptionRequests
                .Include(r => r.Animal)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            ViewBag.User = await _userManager.GetUserAsync(User);
            return View(requests);
        }


        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> Manage(string statusFilter)
        {
            ViewData["StatusFilter"] = statusFilter;

            var requests = _context.AdoptionRequests
                .Include(r => r.Animal)
                .Include(r => r.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
            {
                requests = requests.Where(r => r.Status == statusFilter);
            }

            ViewBag.PendingCount = await _context.AdoptionRequests
                .CountAsync(r => r.Status == "Pending");

            return View(await requests.OrderByDescending(r => r.CreatedAt).ToListAsync());
        }


        [Authorize(Roles = "Manager,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string decision, string? managerComment)
        {
            var request = await _context.AdoptionRequests
                .Include(r => r.Animal)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (decision != "Approved" && decision != "Rejected")
            {
                return BadRequest();
            }

            request.Status = decision;
            request.ManagerComment = string.IsNullOrWhiteSpace(managerComment)
                ? request.ManagerComment
                : managerComment;

            if (decision == "Approved" && request.RequestType == "Adoption"
                && request.Animal != null && request.Animal.Status == "Available")
            {
                request.Animal.Status = "Reserved";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage));
        }
    }
}
