using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Utility;

namespace YetAnotherBugTracker.Roles
{
    public class RolePermissions
    {
        public bool CanAddProjects { get; set; }
        public bool CanAssignUser { get; set; }
        public bool CanDeleteAttachment { get; set; }

        public bool CanEditTicket(ApplicationUser user, string ticketAuthorUserName)
        {
            if(UserIsDeveloper(user) || UserIsDeveloper(user))
                return user.UserName == ticketAuthorUserName;

            return true;
        }

        private bool UserIsStakeHolder(ApplicationUser user)
        {
            return user.Role == DbUtility.Role_Demo_Stakeholder ||
                user.Role == DbUtility.Role_Stakeholder;
        }

        private bool UserIsDeveloper(ApplicationUser user)
        {
            return user.Role == DbUtility.Role_Demo_Developer ||
                user.Role == DbUtility.Role_Developer;
        }

        public bool CanDeleteTicket(ApplicationUser user, string ticketAuthorUserName)
        {
            if(UserIsDeveloper(user))
                return user.UserName == ticketAuthorUserName;

            if(UserIsStakeHolder(user))
                return false;

            return true;
        }
    }
}