using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.Roles
{
    public interface IRoleFactory
    {
        IRole GetRole(ApplicationUser User);
    }
}
