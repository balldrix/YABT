using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Utility;
using YetAnotherBugTracker.ViewModels;

namespace YetAnotherBugTracker.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles = DbUtility.Role_Admin + "," + DbUtility.Role_Demo_Admin)]
        public async Task<ViewResult> Index()
        {
            var viewModel = new UserManagementViewModel
            {
                Users = _userManager.Users.ToList(),
            };

            foreach(var user in viewModel.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Role = roles.First();
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = DbUtility.Role_Admin + "," + DbUtility.Role_Demo_Admin)]
        public IActionResult Create()
        {
            var viewModel = new UserManagementViewModel
            {
                Roles = _roleManager.Roles.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserManagementViewModel viewModel)
        {
            var role = await _roleManager.FindByIdAsync(viewModel.RoleId);

            if(ModelState.IsValid)
            {
                var password = viewModel.Password;

                var user = new ApplicationUser
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    UserName = viewModel.Username,
                };

                user.Role = role.Name;

                var result = await _userManager.CreateAsync(user, password);

                if(result.Succeeded && role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);

                    return RedirectToAction("Index");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var model = new UserManagementViewModel
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Username = viewModel.Username,
                UserId = viewModel.UserId,
                Roles = _roleManager.Roles.ToList(),
                RoleId = role.Id
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = DbUtility.Role_Admin + "," + DbUtility.Role_Demo_Admin)]
        public async Task<IActionResult> Details(string id)
        {
            var user = _userManager.Users.First(u => u.Id == id);
            var roles = await _userManager.GetRolesAsync(user);
            user.Role = roles.First();

            if(user != null)
            {
                var viewModel = new UserManagementViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.UserName,
                    UserId = user.Id,
                    Roles = _roleManager.Roles.ToList(),
                    RoleId = _roleManager.Roles.ToList().First(r => r.Name == user.Role).Id
                };

                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _userManager.Users.First(u => u.Id == id);
            var roles = await _userManager.GetRolesAsync(user);
            user.Role = roles.First();

            // check user is authorized to edit the table
            if(User.IsInRole(DbUtility.Role_Admin) ||
                User.IsInRole(DbUtility.Role_Demo_Admin) ||
                user.UserName == User.Identity.Name)
            {

                if(user != null)
                {
                    var viewModel = new UserManagementViewModel
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Username = user.UserName,
                        UserId = user.Id,
                        Roles = _roleManager.Roles.ToList(),
                        RoleId = _roleManager.Roles.ToList().First(r => r.Name == user.Role).Id,
                        IsEditRoleHidden = true
                    };

                    if(User.IsInRole(DbUtility.Role_Admin) ||
                        User.IsInRole(DbUtility.Role_Demo_Admin))
                    {
                        viewModel.IsEditRoleHidden = false;
                    }

                    return View(viewModel);
                }

                return View("Index");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserManagementViewModel viewModel)
        {
            var user = _userManager.Users.First(u => u.Id == viewModel.UserId);
            if(user != null)
            {
                user.Name = viewModel.Name;
                user.Email = viewModel.Email;
                user.UserName = viewModel.Username;
                var role = await _roleManager.FindByIdAsync(viewModel.RoleId);
                user.Role = role.Name;

                var result = await _userManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    if(userRoles.First() != user.Role)
                    {
                        await _userManager.RemoveFromRoleAsync(user, userRoles.First());
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }

                    return RedirectToAction("Index");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                var model = new UserManagementViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.UserName,
                    UserId = user.Id,
                    Roles = _roleManager.Roles.ToList(),
                    RoleId = role.Id
                };

                return View(model);
            }

            return View("Index");
        }
    }
}
