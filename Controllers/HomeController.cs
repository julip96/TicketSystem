using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using TicketSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;


namespace TicketSystem.Controllers
{


    public class HomeController : Controller
    {

        // Here comes the fun
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<HomeController> _logger;

    

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> LoginAsUser()
        {
            return await Login("user@example.com", "User123!", "User");
        }

        public async Task<IActionResult> LoginAsAdmin()
        {
            return await Login("admin@example.com", "Admin123!", "Admin");
        }

        public async Task<IActionResult> LoginAsDev()
        {
            return await Login("dev@example.com", "Dev123!", "Dev");
        }

        private async Task<IActionResult> Login(string email, string password, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // If the user does not exist, create a new one
                user = new IdentityUser { UserName = email, Email = email };
                await _userManager.CreateAsync(user, password);

                // Assign the user to the specified role
                await _userManager.AddToRoleAsync(user, roleName);
            }

            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index");
        }
    
    // here ends the fun



        [AllowAnonymous]
        public IActionResult Index()
        {

            return View();
        }
        [AllowAnonymous]
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