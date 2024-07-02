using Microsoft.AspNetCore.Identity;
using System.Linq;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Utility;

namespace YetAnotherBugTracker.Roles
{
    public class RoleFactory : IRoleFactory
    {
        private readonly Admin _admin;
        private readonly DemoAdmin _demoAdmin;
        private readonly DemoDeveloper _demoDeveloper;
        private readonly DemoProjectManager _demoProjectManager;
        private readonly DemoStakeholder _demoStakeholder;
        private readonly Developer _developer;
        private readonly ProjectManager _projectManager;
        private readonly Stakeholder _stakeholder;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleFactory(Admin admin,
                            DemoAdmin demoAdmin,
                            DemoDeveloper demoDeveloper,
                            DemoProjectManager demoProjectManager,
                            DemoStakeholder demoStakeholder,
                            Developer developer,
                            ProjectManager projectManager,
                            Stakeholder stakeholder,
                            UserManager<ApplicationUser> userManager
            )
        {
            _admin = admin;
            _demoAdmin = demoAdmin;
            _demoDeveloper = demoDeveloper;
            _demoProjectManager = demoProjectManager;
            _demoStakeholder = demoStakeholder;
            _developer = developer;
            _projectManager = projectManager;
            _stakeholder = stakeholder;
            _userManager = userManager;
        }

        public IRole GetRole(ApplicationUser user)
        {
            var userRoleList = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();

            switch(userRoleList.First())
            {
                case DbUtility.Role_Admin: return _admin;
                case DbUtility.Role_Demo_Admin: return _demoAdmin;
                case DbUtility.Role_Demo_Developer: return _demoDeveloper;
                case DbUtility.Role_Demo_Project_Mananger: return _demoProjectManager;
                case DbUtility.Role_Demo_Stakeholder: return _demoStakeholder;
                case DbUtility.Role_Developer: return _developer;
                case DbUtility.Role_Project_Manager: return _projectManager;
                case DbUtility.Role_Stakeholder: return _stakeholder;
                default: return _stakeholder;
            }
        }
    }
}