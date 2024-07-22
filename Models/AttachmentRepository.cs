using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class AttachmentRepository : IRepository<Attachment>
    {
        private readonly AppDbContext _appDbContext;

        public ICollection<Attachment> AllItems => _appDbContext.Attachment
			.Include(a => a.User)
            .ToList();

        public ICollection<Attachment> DemoItems => throw new System.NotImplementedException();

        public AttachmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(Attachment attachment)
        {
			_appDbContext.Add(attachment);
        }

        public void Delete(int id)
        {
            var attachment = _appDbContext.Attachment.Find(id);
			_appDbContext.Remove(attachment);
        }

        public Attachment Get(int id)
        {
            return _appDbContext.Attachment
				.Include(a => a.User)
                .SingleOrDefault(a => a.Id == id);
        }

        public ICollection<Attachment> Search(string searchTerm)
        {
            return AllItems.Where(a => a.Filename.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
						   .ToList();
        }

        public void Update(Attachment attachment)
        {
			_appDbContext.Update(attachment);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
