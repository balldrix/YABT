using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace YetAnotherBugTracker.Models
{
    public class PriorityRepository : IRepository<Priority>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<Priority> _dbSet;

        public IEnumerable<Priority> AllItems => _appDbContext.Priority;

        public IEnumerable<Priority> DemoItems => throw new System.NotImplementedException();

        public PriorityRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Priority;
        }

        public void Add(Priority item)
        {
            _dbSet.Add(item);
        }

        public void Delete(int id)
        {
            var priority = _dbSet.Find(id);
            _dbSet.Remove(priority);
        }

        public Priority Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(Priority item)
        {
            _dbSet.Update(item);
        }

        public IEnumerable<Priority> Search(string searchTerm)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoSave()
        {
            throw new System.NotImplementedException();
        }

        public void DemoDelete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void DemoAdd(Priority project)
        {
            throw new System.NotImplementedException();
        }

        public Priority DemoGet(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Priority> DemoSearch(string searchTerm)
        {
            throw new System.NotImplementedException();
        }

        public void DemoUpdate(Priority project)
        {
            throw new System.NotImplementedException();
        }
    }
}
