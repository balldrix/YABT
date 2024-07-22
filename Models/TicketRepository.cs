using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly AppDbContext _appDbContext;
        public ICollection<Ticket> AllItems => _appDbContext.Ticket
			.Include(ticket => ticket.Author)
            .Include(ticket => ticket.Priority)
            .Include(ticket => ticket.State)
            .Include(ticket => ticket.Project)
            .Include(ticket => ticket.Type)
            .Include(ticket => ticket.AssignedUser)
            .Include(ticket => ticket.Comments)
            .Include(ticket => ticket.Attachments)
                .ThenInclude(a => a.User).ToList();


        public TicketRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ICollection<Ticket> Search(string searchTerm)
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
                    System.StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void Add(Ticket ticket)
        {
			_appDbContext.Add(ticket);
        }

        public void Delete(int id)
        {
            var ticket = _appDbContext.Ticket.Find(id);
			_appDbContext.Remove(ticket);
        }

        public void Update(Ticket item)
        {
			_appDbContext.Update(item);
        }

        public Ticket Get(int id)
        {
            return _appDbContext.Ticket
				.Include(ticket => ticket.Author)
                .Include(ticket => ticket.Priority)
                .Include(ticket => ticket.State)
                .Include(ticket => ticket.Project)
                .Include(ticket => ticket.Type)
                .Include(ticket => ticket.AssignedUser)
                .Include(ticket => ticket.Comments)
                .Include(ticket => ticket.Attachments)
                    .ThenInclude(a => a.User)
                .SingleOrDefault(t => t.Id == id);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
