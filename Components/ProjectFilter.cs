using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;
using YetAnotherBugTracker.Utility;

namespace YetAnotherBugTracker.Components
{
    public class ProjectFilter : ViewComponent
    {
        private readonly IRoleFactory _roleFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectFilter(IRoleFactory roleFactory,
            UserManager<ApplicationUser> userManager)
        {
            _roleFactory = roleFactory;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal user)
        {
            var applicationUser = await _userManager.GetUserAsync(user);
            var roleObject = _roleFactory.GetRole(applicationUser);

            var projects = roleObject.GetProjectsForUserRole(applicationUser);

            return View(projects);
        }
    }
}
