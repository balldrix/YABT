using System.Collections.Generic;

namespace YetAnotherBugTracker.Models
{
    public interface IRepository<T>
    {
		ICollection<T> AllItems { get; }

        void Add(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        void Save();
		ICollection<T> Search(string searchTerm);
    }
}