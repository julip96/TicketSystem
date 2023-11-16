
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{

    // HERE IS HOW TO AUTHORIZE ONLY ADMINS
    [Authorize(Roles = "admin, dev")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ManageUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // Show all users
		public IActionResult Index()
        {
			var allUsers = _userManager.Users.ToList();
			return View(allUsers);
		}

        public async Task<IActionResult> UserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            ViewBag.AllRoles = _roleManager.Roles.ToList();

            // Pass user and roles to the view
            ViewBag.Roles = userRoles;
            return View(user);
        }

        // Show single user and edit
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            var model = new ManageRolesViewModel
            {
                UserId = userId,
                UserRoles = userRoles.ToList(),
                AllRoles = allRoles
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Remove user from existing roles
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Add user to selected roles
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);

            // Redirect to the user details page
            return RedirectToAction("UserDetails", new { userId = model.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoles(string id, string[] roles)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Get the current roles for the user
            var existingRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from existing roles
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            // Add the user to the selected roles
            await _userManager.AddToRolesAsync(user, roles);

            return RedirectToAction("UserDetails", new { userId = id });
        }

    }
}