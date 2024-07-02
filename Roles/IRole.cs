using System.Collections.Generic;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.Roles
{
    public interface IRole
    {
        RolePermissions Permissions { get; set; }
        void AddNewProject(Project project);
        void AddNewTicket(Ticket newTicket);
        void DeleteUserProject(ApplicationUser user, int id);
        void DeleteUserTicket(ApplicationUser user, int id);
        Project GetUserProject(ApplicationUser user, int id);
        Ticket GetUserTicket(ApplicationUser user, int id);
        IEnumerable<Project> GetProjectsForUserRole(ApplicationUser user);
        IEnumerable<Ticket> GetTicketsForUserRole(ApplicationUser user);
        IEnumerable<Project> SearchUserProjects(ApplicationUser user, string searchTerm);
        IEnumerable<Ticket> SearchUserTickets(ApplicationUser user, string searchTerm);
        void UpdateProject(Project project);
        void UpdateTicket(Ticket ticket);
    }
}