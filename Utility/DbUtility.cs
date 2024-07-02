using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.Utility
{
    public static class DbUtility
    {
        public const string Demo_Project = "Demo Project";
        public const string Role_Admin = "Admin";
        public const string Role_Project_Manager = "Project Manager";
        public const string Role_Developer = "Developer";
        public const string Role_Stakeholder = "Stakeholder";
        public const string Role_Demo_Admin = "Demo Admin";
        public const string Role_Demo_Project_Mananger = "Demo Project Manager";
        public const string Role_Demo_Developer = "Demo Developer";
        public const string Role_Demo_Stakeholder = "Demo Stakeholder";

        public static bool IsDemoUser(ClaimsPrincipal user)
        {
            if(user.IsInRole(Role_Demo_Admin) ||
                user.IsInRole(Role_Demo_Project_Mananger) ||
                user.IsInRole(Role_Demo_Developer) ||
                user.IsInRole(Role_Demo_Stakeholder))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<IList<ApplicationUser>> GetProjectManagers(UserManager<ApplicationUser> userManager)
        {
            var projectManagers = await userManager.GetUsersInRoleAsync(DbUtility.Role_Project_Manager);

            var demoProjectManager = userManager.Users.First(u => u.UserName == "Demo_ProjectManager");
            projectManagers.Add(demoProjectManager);
            return projectManagers;
        }
    }
}
