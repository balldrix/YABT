using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly AppDbContext _appDbContext;

        public ICollection<Comment> AllItems => _appDbContext.Comment
			.Include(c => c.User)
			.OrderBy(c => c.Date)
			.ToList();

        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Comment item)
        {
			_appDbContext.Add(item);
        }

        public void Delete(int id)
        {
            var status = _appDbContext.Comment.Find(id);
			_appDbContext.Remove(status);
        }

        public Comment Get(int id)
        {
            return _appDbContext.Comment
				.Include(c => c.User)
                .SingleOrDefault(c => c.Id == id);
        }

        public void Update(Comment comment)
        {
            _appDbContext.Update(comment);
		}

        public ICollection<Comment> Search(string searchTerm)
        {
			return AllItems.Where(a => a.TextComment.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
							.ToList();
		}

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
