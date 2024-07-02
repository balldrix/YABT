using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DemoDbContext _demoDbContext;
        private readonly DbSet<Ticket> _dbSet;
        private readonly DbSet<Ticket> _demoDbSet;

        public IEnumerable<Ticket> AllItems => _dbSet
            .Include(ticket => ticket.Author)
            .Include(ticket => ticket.Priority)
            .Include(ticket => ticket.State)
            .Include(ticket => ticket.Project)
            .Include(ticket => ticket.Type)
            .Include(ticket => ticket.AssignedUser)
            .Include(ticket => ticket.Comments)
                .ThenInclude(c => c.User)
            .Include(ticket => ticket.Attachments)
                .ThenInclude(a => a.User);

        public IEnumerable<Ticket> DemoItems => _demoDbSet
            .Include(ticket => ticket.Author)
            .Include(ticket => ticket.Priority)
            .Include(ticket => ticket.State)
            .Include(ticket => ticket.Project)
            .Include(ticket => ticket.Type)
            .Include(ticket => ticket.AssignedUser)
            .Include(ticket => ticket.Comments)
                .ThenInclude(c => c.User)
            .Include(ticket => ticket.Attachments)
                .ThenInclude(a => a.User);

        public TicketRepository(AppDbContext appDbContext,
                                DemoDbContext demoDbContext)
        {
            _appDbContext = appDbContext;
            _demoDbContext = demoDbContext;
            _dbSet = _appDbContext.Set<Ticket>();
            _demoDbSet = _demoDbContext.Set<Ticket>();
        }

        public IEnumerable<Ticket> Search(string searchTerm)
        {
            return AllItems
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

        public void Add(Ticket ticket)
        {
            _dbSet.Add(ticket);
        }

        public void Delete(int id)
        {
            var ticket = _dbSet.Find(id);
            _dbSet.Remove(ticket);
        }

        public void Update(Ticket item)
        {
            _dbSet.Update(item);
        }

        public Ticket Get(int id)
        {
            return _dbSet
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
                .SingleOrDefault(t => t.Id == id);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoAdd(Ticket ticket)
        {
            _demoDbSet.Add(ticket);
        }

        public void DemoDelete(int id)
        {
            var ticket = _demoDbSet.Find(id);
            _demoDbSet.Remove(ticket);
        }

        public void DemoUpdate(Ticket item)
        {
            _demoDbSet.Update(item);
        }

        public Ticket DemoGet(int id)
        {
            return _demoDbSet
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
                .SingleOrDefault(t => t.Id == id);
        }

        public void DemoSave()
        {
            _demoDbContext.SaveChanges();
        }

        public IEnumerable<Ticket> DemoSearch(string searchTerm)
        {
            return DemoItems
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
        }
}
