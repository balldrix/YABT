using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class StateRepository : IRepository<State>
    {
        private readonly AppDbContext _appDbContext;

        public ICollection<State> AllItems => _appDbContext.State.ToList();

        public StateRepository(AppDbContext appDbContext)
        {
			_appDbContext = appDbContext;
        }

        public void Add(State item)
        {
			_appDbContext.Add(item);
        }

        public void Delete(int id)
        {
            var status = _appDbContext.State.Find(id);
			_appDbContext.Remove(status);
        }

        public State Get(int id)
        {
            return _appDbContext.State.Find(id);
        }

        public void Update(State item)
        {
			_appDbContext.Update(item);
        }

        public ICollection<State> Search(string searchTerm)
        {
            return _appDbContext.State.Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
