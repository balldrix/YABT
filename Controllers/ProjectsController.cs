using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Roles;
using YetAnotherBugTracker.Utility;
using YetAnotherBugTracker.ViewModels;

namespace YetAnotherBugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IRoleFactory _roleFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(IRoleFactory roleFactory,
                                  UserManager<ApplicationUser> userManager)
        {
            _roleFactory = roleFactory;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<ViewResult> Index()
        {
            var projectManagers = await DbUtility.GetProjectManagers(_userManager);
            var viewModel = InitializeViewModelWithProjectManagers(projectManagers);

            return View(viewModel);
        }

        private ProjectsViewModel InitializeViewModelWithProjectManagers(IList<ApplicationUser> projectManagers)
        {
            var applicationUser = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var roleObject = _roleFactory.GetRole(applicationUser);

            var viewModel = new ProjectsViewModel
            {
                ProjectLeadOptions = projectManagers
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.Name
                    })
                    .ToList(),
                CanAddProjects = roleObject.Permissions.CanAddProjects,
                Projects = roleObject.GetProjectsForUserRole(applicationUser)
            };

            viewModel.ProjectLeadOptions.Add(GetEmptyOption());

            return viewModel;
        }

        private static SelectListItem GetEmptyOption()
        {
            return new SelectListItem
            {
                Value = "",
                Text = "None",
                Selected = true
            };
        }

        [HttpPost]
        public async Task<ActionResult> AddProject(ProjectsViewModel projectViewModel)
        {
            if(ModelState.IsValid)
            {
                var project = new Project
                {
                    Name = projectViewModel.Project.Name,
                    ProjectLead = _userManager.Users.Single(p => p.Id == projectViewModel.ProjectLeadId),
                    Author = _userManager.Users.First(u => u.UserName == User.Identity.Name)
                };

                var applicationUser = await _userManager.GetUserAsync(User);
                var roleObject = _roleFactory.GetRole(applicationUser);
                roleObject.AddNewProject(project);

                return RedirectToAction("Index", "Tickets", new { id = project.Id });
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeLead(ProjectsViewModel projectsViewModel)
        {
            if(ModelState.IsValid)
            {
                var applicationUser = await _userManager.GetUserAsync(User);
                var roleObject = _roleFactory.GetRole(applicationUser);
                
                var project = roleObject.GetUserProject(applicationUser,
                    projectsViewModel.Project.Id);
                
                project.ProjectLead = _userManager.Users.FirstOrDefault(p => p.Id == projectsViewModel.ProjectLeadId);

                roleObject.UpdateProject(project);

                return RedirectToAction("Index");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);
            roleObject.DeleteUserProject(applicationUser, id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Rename(ProjectsViewModel projectViewModel)
        {
            var id = projectViewModel.Project.Id;
            var newName = projectViewModel.Project.Name;
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            Project project = roleObject.GetUserProject(applicationUser, id);
            project.Name = newName;
            roleObject.UpdateProject(project);

            return RedirectToAction("Details", project);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            var project = roleObject.GetUserProject(applicationUser, id);

            foreach(var member in project.Members)
            {
                var roles = await _userManager.GetRolesAsync(member);
                member.Role = roles.First();
            }

            var viewModel = new ProjectsViewModel
            {
                Project = project,
                CanAmendMembers = true
            };

            if(User.IsInRole(DbUtility.Role_Stakeholder) ||
                User.IsInRole(DbUtility.Role_Demo_Stakeholder) ||
                User.IsInRole(DbUtility.Role_Demo_Developer) ||
                User.IsInRole(DbUtility.Role_Developer))
            {
                viewModel.CanAmendMembers = false;
            }

            var options = _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Name
                }).ToList();

            foreach(var option in options.ToArray())
            {
                foreach(var member in viewModel.Project.Members)
                {
                    if(option.Value == member.Id)
                    {
                        options.Remove(option);
                    }
                }
            }

            viewModel.MemberOptions = new MultiSelectList(options, "Value", "Text", viewModel.SelectedMembers = new string[options.Count]);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMembers(ProjectsViewModel viewModel)
        {
            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            var project = roleObject.GetUserProject(applicationUser, viewModel.Project.Id);

            foreach(var selection in viewModel.SelectedMembers)
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == selection);
                project.Members.Add(user);
            }

            roleObject.UpdateProject(project);            

            return RedirectToAction("Details", new { id = project.Id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMember(ProjectsViewModel viewModel)
        {
            var user = _userManager.Users.First(u => u.Id == viewModel.MemberId);

            if(user != null)
            {
                var applicationUser = await _userManager.GetUserAsync(User);
                var roleObject = _roleFactory.GetRole(applicationUser);
                var project = roleObject.GetUserProject(applicationUser, viewModel.Project.Id);

                project.Members.Remove(user);
                roleObject.UpdateProject(project);
            }

            return RedirectToAction("Details", new { id = viewModel.Project.Id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if(searchTerm == null)
            {
                return RedirectToAction("Index");
            }

            var projectManagers = await DbUtility.GetProjectManagers(_userManager);
            var viewModel = InitializeViewModelWithProjectManagers(projectManagers);

            var currentUser = _userManager.Users.First(u => u.UserName == User.Identity.Name);

            var applicationUser = await _userManager.GetUserAsync(User);
            var roleObject = _roleFactory.GetRole(applicationUser);

            viewModel.Projects = roleObject.SearchUserProjects(currentUser, searchTerm);

            return View("Index", viewModel);
        }
    }
}