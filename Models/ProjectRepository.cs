using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly AppDbContext _appDbContext;

        public ICollection<Project> AllItems => _appDbContext.Project
            .Include(p => p.Members)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.State)
			.Include(p => p.Tickets)
				.ThenInclude(t => t.Type)
			.Include(p => p.Tickets)
				.ThenInclude(t => t.Priority)
			.ToList();


        public ProjectRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Project project)
        {
			_appDbContext.Add(project);
        }

        public void Delete(int id)
        {
            var project = _appDbContext.Project.Find(id);
			_appDbContext.Remove(project);
        }

        public void Update(Project item)
        {
			_appDbContext.Update(item);
        }

        public Project Get(int id)
        {
            return _appDbContext.Project
				.Include(p => p.Author)
                .Include(p => p.Members)
                .Include(p => p.ProjectLead)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.AssignedUser)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Priority)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.State)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Project)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Type)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Comments)
                    .ThenInclude(c => c.User)
                .SingleOrDefault(p => p.Id == id);
        }

        public ICollection<Project> Search(string searchTerm)
        {
            return AllItems
                .Where(p =>
               (p.Author != null && p.Author.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               (p.ProjectLead != null && p.ProjectLead.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               p.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
