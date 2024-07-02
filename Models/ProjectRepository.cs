using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DemoDbContext _demoDbContext;
        private readonly DbSet<Project> _dbSet;
        private readonly DbSet<Project> _demoDbSet;

        public IEnumerable<Project> AllItems => _dbSet
            .Include(p => p.Author)
            .Include(p => p.ProjectLead)
            .Include(p => p.Members)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Priority)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.State)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Project)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Type)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.AssignedUser)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Comments)
                .ThenInclude(c => c.User);

        public IEnumerable<Project> DemoItems => _demoDbSet
            .Include(p => p.Author)
            .Include(p => p.ProjectLead)
            .Include(p => p.Members)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Priority)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.State)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Project)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Type)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.AssignedUser)
            .Include(p => p.Tickets)
                .ThenInclude(t => t.Comments)
                .ThenInclude(c => c.User);

        public ProjectRepository(AppDbContext appDbContext,
                                 DemoDbContext demoDbContext)
        {
            _appDbContext = appDbContext;
            _demoDbContext = demoDbContext;
            _dbSet = _appDbContext.Set<Project>();
            _demoDbSet = _demoDbContext.Set<Project>();
        }

        public void Add(Project project)
        {
            _dbSet.Add(project);
        }

        public void DemoAdd(Project project)
        {
            _demoDbSet.Add(project);
        }

        public void Delete(int id)
        {
            var project = _dbSet.Find(id);
            _dbSet.Remove(project);
        }

        public void DemoDelete(int id)
        {
            var project = _demoDbSet.Find(id);
            _demoDbSet.Remove(project);
        }

        public void Update(Project item)
        {
            _dbSet.Update(item);
        }

        public void DemoUpdate(Project item)
        {
            _demoDbSet.Update(item);
        }

        public Project Get(int id)
        {
            return _dbSet
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

        public Project DemoGet(int id)
        {
            return _demoDbSet
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

        public IEnumerable<Project> Search(string searchTerm)
        {
            return AllItems
                .Where(p =>
               (p.Author != null && p.Author.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               (p.ProjectLead != null && p.ProjectLead.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               p.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Project> DemoSearch(string searchTerm)
        {
            return DemoItems
                .Where(p =>
               (p.Author != null && p.Author.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               (p.ProjectLead != null && p.ProjectLead.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase)) ||
               p.Name.Contains(searchTerm,
               System.StringComparison.OrdinalIgnoreCase));
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoSave()
        {
            _demoDbContext.SaveChanges();
        }
    }
}
