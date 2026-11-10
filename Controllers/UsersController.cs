using AnimalSanctuary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalSanctuary.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users
                .OrderBy(u => u.RegistrationDate)
                .ToList();

            var rows = new List<UserRowViewModel>();
            foreach (var user in users)
            {
                rows.Add(new UserRowViewModel
                {
                    User = user,
                    IsManager = await _userManager.IsInRoleAsync(user, "Manager"),
                    IsAdministrator = await _userManager.IsInRoleAsync(user, "Administrator")
                });
            }

            return View(rows);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleManager(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                TempData["UsersError"] = "The Administrator role cannot be changed here.";
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, "Manager"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Manager");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Manager");
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class UserRowViewModel
    {
        public ApplicationUser User { get; set; } = new();
        public bool IsManager { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
