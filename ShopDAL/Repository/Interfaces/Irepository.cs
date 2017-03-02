using System.Collections.Generic;

namespace ShopDAL.Repository.Interfaces
{
    public interface IRepository<T>
    {
        List<T> ReadAll();

        void Add(T item);

        T Find(int? id);
        void Delete(int id);

        void Edit(T item);
        T FindByName(string name);

        string FindName(string name);
    }
}
