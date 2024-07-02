using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;
using YetAnotherBugTracker.Utility;
using YetAnotherBugTracker.ViewModels;

namespace YetAnotherBugTracker.Components
{
    public class ProjectLeadDropdown : ViewComponent
    {
        private readonly IRoleFactory _roleFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectLeadDropdown(IRoleFactory roleFactory,
            UserManager<ApplicationUser> userManager)
        {
            _roleFactory = roleFactory;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId, string projectLeadId, ClaimsPrincipal user)
        {
            if(projectLeadId == null)
            {
                projectLeadId = "";
            }

            var applicationUser = _userManager.GetUserAsync(user).GetAwaiter().GetResult();
            var roleObject = _roleFactory.GetRole(applicationUser);

            var viewModel = new ProjectsViewModel
            {
                ProjectLeadId = projectLeadId,
                Project = roleObject.GetUserProject(applicationUser, projectId)
            };

            var projectManagers = await DbUtility.GetProjectManagers(_userManager);

            viewModel.ProjectLeadOptions = projectManagers
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Name
                })
                .ToList();

            var emptyOption = new SelectListItem
            {
                Value = "",
                Text = "None"
            };

            viewModel.ProjectLeadOptions.Add(emptyOption);

            foreach(var option in viewModel.ProjectLeadOptions)
            {
                if(option.Value == projectLeadId)
                {
                    option.Selected = true;
                }
            }

            return View(viewModel);
        }
    }
}
