using ShopDAL;
using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBLL.Services
{   
    public class CategoryService
    {
        private FacadeDAL facade = new FacadeDAL();


        public IEnumerable<Category> ReadAll()
        {

            List<Category> categories = facade.GetCategoryRepository().ReadAll();
            return categories;
        }

        public IEnumerable<Category> GetParentByPageName(string pageName)
        {
            Category category = facade.GetCategoryRepository().FindByName(pageName);
            List<Category> parents = facade.GetCategoryRepository2().GetParentCategoryById(category.id);
            return parents;
        }

        public IEnumerable<Category> GetChildren()
        {

            List<Category> children = new List<Category>();
            foreach (var cat in ReadAll())
            {
                children = facade.GetCategoryRepository2().GetSubCategories(cat.id);
            }

            return children;

        }

        public IEnumerable<Category> GetChildByPageName(string pageName)
        {

            Category category = facade.GetCategoryRepository().FindByName(pageName);

            List<Category> childs = facade.GetCategoryRepository2().GetSubCategories(category.id);

            return childs;
        }

        public Category GetDetails(string pageName)
        {

            Category category = facade.GetCategoryRepository().FindByName(pageName);

            return category;
        }

        public string FindName(string categoryName)
        {
            return facade.GetCategoryRepository().FindName(categoryName);
        }
        public Category FindByName(string categoryName)
        {
            return facade.GetCategoryRepository().FindByName(categoryName);
        }
        public void AddCategory(Category category)
        {
            facade.GetCategoryRepository().Add(category);
        }

        public void AddCategoryWithId(int id, Category category)
        {
            facade.GetCategoryRepository2().AddWithId(id, category);
        }

        public Category Find(int? id)
        {
            return facade.GetCategoryRepository().Find(id);
        }

        public void Update(int id, Category category)
        {
            category.id = id;
            facade.GetCategoryRepository().Edit(category);

        }

        public void Delete(int id)
        {

            facade.GetCategoryRepository().Delete(id);
        }

        public IEnumerable<Category> GetParentCategories()
        {
            return facade.GetCategoryRepository2().GetAllParentCategory();
        }
    }
}
