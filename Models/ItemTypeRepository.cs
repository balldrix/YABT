using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherBugTracker.Models
{
	public class ItemTypeRepository : IRepository<ItemType>
	{
		private readonly AppDbContext _appDbContext;

		public ICollection<ItemType> AllItems => _appDbContext.ItemType.ToList();

		public ItemTypeRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public void Add(ItemType item)
		{
			_appDbContext.Add(item);
		}

		public void Delete(int id)
		{
			var itemType = _appDbContext.ItemType.Find(id);
			_appDbContext.Remove(itemType);
		}

		public ItemType Get(int id)
		{
			return _appDbContext.ItemType.Find(id);
		}

		public void Update(ItemType item)
		{
			_appDbContext.Update(item);
		}

		public ICollection<ItemType> Search(string searchTerm)
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
