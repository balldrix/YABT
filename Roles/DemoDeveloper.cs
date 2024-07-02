using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Utility;

namespace YetAnotherBugTracker.Roles
{
    public class DemoDeveloper : IRole
    {
        private readonly DemoDbContext _demoDbContext;

        public RolePermissions Permissions { get; set; }

        public DemoDeveloper(DemoDbContext demoDbContext)
        {
            _demoDbContext = demoDbContext;

            Permissions = new RolePermissions()
            {
                CanAddProjects = false,
                CanAssignUser = true,
                CanDeleteAttachment = false,
            };
        }

        public IEnumerable<Project> GetProjectsForUserRole(ApplicationUser user)
        {
            return _demoDbContext.Project
                .Include(p => p.Author)
                .Include(p => p.ProjectLead)
                .Include(p => p.Members)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Priority)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.State)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Type)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.AssignedUser)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Where(p => p.Members.Contains(user));
        }

        public IEnumerable<Ticket> GetTicketsForUserRole(ApplicationUser user)
        {
            return _demoDbContext.Ticket
                .Include(ticket => ticket.Author)
                .Include(ticket => ticket.Priority)
                .Include(ticket => ticket.State)
                .Include(ticket => ticket.Project)
                .Include(ticket => ticket.Type)
                .Include(ticket => ticket.AssignedUser)
                .Include(ticket => ticket.Comments)
                    .ThenInclude(c => c.User)
                .Include(ticket => ticket.Attachments)
                    .ThenInclude(a => a.User)
                .AsQueryable()
                .Where(Predicates.DeveloperTickets(user));
        }

        public void AddNewProject(Project project)
        {
            // developers cannot add new projects
        }

        public Project GetUserProject(ApplicationUser user, int id)
        {
            return GetProjectsForUserRole(user)
                .FirstOrDefault(p => p.Id == id);
        }

        public Ticket GetUserTicket(ApplicationUser user, int id)
        {
            return GetTicketsForUserRole(user)
                .FirstOrDefault(p => p.Id == id);
        }

        public void UpdateProject(Project project)
        {
            _demoDbContext.Project.Update(project);
            _demoDbContext.SaveChanges();
        }

        public void DeleteUserProject(ApplicationUser user, int id)
        {
            _demoDbContext.Project.Remove(GetUserProject(user, id));
            _demoDbContext.SaveChanges();
        }

        public void DeleteUserTicket(ApplicationUser user, int id)
        {
            _demoDbContext.Ticket.Remove(GetUserTicket(user, id));
            _demoDbContext.SaveChanges();
        }

        public IEnumerable<Project> SearchUserProjects(ApplicationUser user,
                                               string searchTerm)
        {
            return GetProjectsForUserRole(user)
                .Where(p =>
               (p.Author != null && p.Author.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               (p.ProjectLead != null && p.ProjectLead.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               p.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Ticket> SearchUserTickets(ApplicationUser user, string searchTerm)
        {
            return GetTicketsForUserRole(user)
                .Where(t =>
                (t.AssignedUser != null && t.AssignedUser.Name.Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase)) ||
                (t.Author != null && t.Author.Name.Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase)) ||
                (t.Description != null && t.Description.Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase)) ||
                (t.Project.Name != null && t.Project.Name.Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase)) ||
                (t.Project.Author != null && t.Project.Author.Name.Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase)) ||
                (t.Title != null && t.Title.Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase)) ||
                t.Id.ToString().Contains(searchTerm,
                    System.StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateTicket(Ticket ticket)
        {
            _demoDbContext.Ticket.Update(ticket);
            _demoDbContext.SaveChanges();
        }

        public void AddNewTicket(Ticket newTicket)
        {
            _demoDbContext.Ticket.Add(newTicket);
            _demoDbContext.SaveChanges();
        }
    }
}