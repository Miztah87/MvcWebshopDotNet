using System.Collections.Generic;

namespace ShopDAL.Repository.Interfaces
{
    public interface ICategory<T>
    {
        void AddWithId(int id, T item);

        List<T> GetChilds();

        List<T> GetSubCategories(int id);

        List<T> GetAllParentCategory();

        List<T> GetParentCategoryById(int id);

        T Read(int id);
        //void SaveImgPath(string path, T cat);
    }
}
