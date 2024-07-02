using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
    public class AttachmentRepository : IRepository<Attachment>
    {
        private readonly AppDbContext _appDbContext;
        private DbSet<Attachment> _attachmentDbSet;

        public IEnumerable<Attachment> AllItems => _appDbContext.Attachment
            .Include(a => a.User);

        public IEnumerable<Attachment> DemoItems => throw new System.NotImplementedException();

        public AttachmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _attachmentDbSet = _appDbContext.Attachment;
        }

        public void Add(Attachment attachment)
        {
            _attachmentDbSet.Add(attachment);
        }

        public void Delete(int id)
        {
            var attachment = _attachmentDbSet.Find(id);
            _attachmentDbSet.Remove(attachment);
        }

        public Attachment Get(int id)
        {
            return _attachmentDbSet
                .Include(a => a.User)
                .SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<Attachment> Search(string searchTerm)
        {
            return AllItems.Where(a => a.Filename.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase));
        }

        public void Update(Attachment attachment)
        {
            _attachmentDbSet.Update(attachment);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void DemoAdd(Attachment project)
        {
            throw new System.NotImplementedException();
        }

        public void DemoDelete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Attachment DemoGet(int id)
        {
            throw new System.NotImplementedException();
        }

        public void DemoSave()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Attachment> DemoSearch(string searchTerm)
        {
            throw new System.NotImplementedException();
        }

        public void DemoUpdate(Attachment project)
        {
            throw new System.NotImplementedException();
        }
    }
}
