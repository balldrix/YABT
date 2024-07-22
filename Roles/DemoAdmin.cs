using System.Collections.Generic;
using System.Linq;
using YetAnotherBugTracker.Models;

namespace YetAnotherBugTracker.Roles
{
	public class DemoAdmin : IRole
	{
		private readonly IRepository<Ticket> _ticketRepository;
		private readonly IRepository<Project> _projectRepository;
		public RolePermissions Permissions { get; set; }

		public DemoAdmin(IRepository<Project> projectRepository, IRepository<Ticket> ticketRepository)
		{
			_projectRepository = projectRepository;
			_ticketRepository = ticketRepository;

			Permissions = new RolePermissions()
			{
				CanAddProjects = true,
				CanAssignUser = true,
				CanDeleteAttachment = true,
			};
		}

		public ICollection<Project> GetProjectsForUserRole(ApplicationUser user)
		{
			return _projectRepository.AllItems;
		}

		public ICollection<Ticket> GetTicketsForUserRole(ApplicationUser user)
		{
			return _ticketRepository.AllItems;
		}

		public void AddNewProject(Project project)
		{
			_projectRepository.Add(project);
			_projectRepository.Save();
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
			_projectRepository.Update(project);
			_projectRepository.Save();
		}

		public void DeleteUserProject(ApplicationUser user, int id)
		{
			_projectRepository.Delete(id);
			_projectRepository.Save();
		}

		public void DeleteUserTicket(ApplicationUser user, int id)
		{
			_ticketRepository.Delete(id);
			_ticketRepository.Save();
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
			   System.StringComparison.OrdinalIgnoreCase)).ToList();
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
					System.StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public void UpdateTicket(Ticket ticket)
		{
			_ticketRepository.Update(ticket);
			_ticketRepository.Save();
		}

		public void AddNewTicket(Ticket ticket)
		{
			_ticketRepository.Add(ticket);
			_ticketRepository.Save();
		}
	}
}