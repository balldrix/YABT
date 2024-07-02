using System.Collections.Generic;

namespace YetAnotherBugTracker.Models
{
    public interface IRepository<T>
    {
        IEnumerable<T> AllItems { get; }
        IEnumerable<T> DemoItems { get; }

        void Add(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        void Save();
        IEnumerable<T> Search(string searchTerm);
        void DemoAdd(T project);
        void DemoDelete(int id);
        T DemoGet(int id);
        void DemoSave();
        IEnumerable<T> DemoSearch(string searchTerm);
        void DemoUpdate(T project);
    }
}