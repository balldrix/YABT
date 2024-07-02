using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<Comment> _dbSet;

        public IEnumerable<Comment> AllItems => _appDbContext.Comment
            .Include(c => c.User).OrderBy(c => c.Date);

        public IEnumerable<Comment> DemoItems => throw new NotImplementedException();

        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Comment;
        }

        public void Add(Comment item)
        {
            _dbSet.Add(item);
        }

        public void Delete(int id)
        {
            var status = _dbSet.Find(id);
            _dbSet.Remove(status);
        }

        public Comment Get(int id)
        {
            return _dbSet
                .Include(c => c.User)
                .SingleOrDefault(c => c.Id == id);
        }

        public void Update(Comment comment)
        {
            _dbSet.Update(comment);
        }

        public IEnumerable<Comment> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoAdd(Comment project)
        {
            throw new NotImplementedException();
        }

        public void DemoDelete(int id)
        {
            throw new NotImplementedException();
        }

        public Comment DemoGet(int id)
        {
            throw new NotImplementedException();
        }

        public void DemoSave()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> DemoSearch(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public void DemoUpdate(Comment project)
        {
            throw new NotImplementedException();
        }
    }
}
