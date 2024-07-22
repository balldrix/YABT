using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using YetAnotherBugTracker.Models;
using YetAnotherBugTracker.Utility;

namespace YetAnotherBugTracker.Roles
{
    public class Developer : IRole
    {
        private readonly AppDbContext _appDbContext;
        public RolePermissions Permissions { get; set; }

        public Developer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            Permissions = new RolePermissions()
            {
                CanAddProjects = false,
                CanAssignUser = true,
                CanDeleteAttachment = false,
            };
        }

        public ICollection<Project> GetProjectsForUserRole(ApplicationUser user)
        {
            return _appDbContext.Project
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
                .Where(p => p.Members.Contains(user))
                .ToList();
        }

        public ICollection<Ticket> GetTicketsForUserRole(ApplicationUser user)
        {
            return _appDbContext.Ticket
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
                .Where(Predicates.DeveloperTickets(user))
                .ToList();
        }

        public void AddNewProject(Project project)
        {
            // devs cannot add new projects
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
            _appDbContext.Project.Update(project);
            _appDbContext.SaveChanges();
        }

        public void DeleteUserProject(ApplicationUser user, int id)
        {
            _appDbContext.Project.Remove(GetUserProject(user, id));
            _appDbContext.SaveChanges();
        }

        public void DeleteUserTicket(ApplicationUser user, int id)
        {
            _appDbContext.Ticket.Remove(GetUserTicket(user, id));
            _appDbContext.SaveChanges();
        }

        public ICollection<Project> SearchUserProjects(ApplicationUser user,
                                               string searchTerm)
        {
            return GetProjectsForUserRole(user)
                .Where(p =>
               (p.Author != null && p.Author.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               (p.ProjectLead != null && p.ProjectLead.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               p.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public ICollection<Ticket> SearchUserTickets(ApplicationUser user, string searchTerm)
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
                    System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void UpdateTicket(Ticket ticket)
        {
            _appDbContext.Ticket.Update(ticket);
            _appDbContext.SaveChanges();
        }

        public void AddNewTicket(Ticket newTicket)
        {
            _appDbContext.Ticket.Add(newTicket);
            _appDbContext.SaveChanges();
        }
	}
}