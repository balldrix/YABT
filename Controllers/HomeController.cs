using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;
using YetAnotherBugTracker.ViewModels;

namespace YetAnotherBugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoleFactory _roleFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IRoleFactory roleFactory,
                              UserManager<ApplicationUser> userManager)
        {
            _roleFactory = roleFactory;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel();
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            viewModel.Projects = roleObject.GetProjectsForUserRole(applicationUser);
            viewModel.Tickets = roleObject.GetTicketsForUserRole(applicationUser).OrderBy(t => t.PriorityID);

            return View(viewModel);
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
