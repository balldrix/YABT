using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace YetAnotherBugTracker.Models
{
    public class StateRepository : IRepository<State>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<State> _dbSet;

        public IEnumerable<State> AllItems => _appDbContext.State;

        public IEnumerable<State> DemoItems => throw new NotImplementedException();

        public StateRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.State;
        }

        public void Add(State item)
        {
            _dbSet.Add(item);
        }

        public void Delete(int id)
        {
            var status = _dbSet.Find(id);
            _dbSet.Remove(status);
        }

        public State Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(State item)
        {
            _dbSet.Update(item);
        }

        public IEnumerable<State> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoAdd(State project)
        {
            throw new NotImplementedException();
        }

        public void DemoDelete(int id)
        {
            throw new NotImplementedException();
        }

        public State DemoGet(int id)
        {
            throw new NotImplementedException();
        }

        public void DemoSave()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<State> DemoSearch(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void DemoUpdate(State project)
        {
            throw new NotImplementedException();
        }
    }
}
