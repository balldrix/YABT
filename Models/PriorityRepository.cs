using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class PriorityRepository : IRepository<Priority>
    {
        private readonly AppDbContext _appDbContext;

        public ICollection<Priority> AllItems => _appDbContext.Priority.ToList();

        public PriorityRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Priority item)
        {
			_appDbContext.Add(item);
        }

        public void Delete(int id)
        {
            var priority = _appDbContext.Priority.Find(id);
			_appDbContext.Remove(priority);
        }

        public Priority Get(int id)
        {
            return _appDbContext.Priority.Find(id);
        }

        public void Update(Priority item)
        {
			_appDbContext.Update(item);
        }

        public ICollection<Priority> Search(string searchTerm)
        {
			return AllItems.Where(a => a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
				.ToList();
		}

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
