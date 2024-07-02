using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace YetAnotherBugTracker.Models
{
    public class ItemTypeRepository : IRepository<ItemType>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<ItemType> _dbSet;

        public IEnumerable<ItemType> AllItems => _appDbContext.ItemType;

        public IEnumerable<ItemType> DemoItems => throw new NotImplementedException();

        public ItemTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.ItemType;
        }

        public void Add(ItemType item)
        {
            _dbSet.Add(item);
        }

        public void Delete(int id)
        {
            var itemType = _dbSet.Find(id);
            _dbSet.Remove(itemType);
        }

        public ItemType Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(ItemType item)
        {
            _dbSet.Update(item);
        }

        public IEnumerable<ItemType> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoAdd(ItemType item)
        {
            throw new NotImplementedException();
        }

        public void DemoSave()
        {
            throw new NotImplementedException();
        }

        public void DemoUpdate(ItemType item)
        {
            throw new NotImplementedException();
        }

        public void DemoDelete(int id)
        {
            throw new NotImplementedException();
        }

        public ItemType DemoGet(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemType> DemoSearch(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
