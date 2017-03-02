
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using ShopDAL.Repository.Interfaces;
using ShopDAL.DomainModel;
using ShopDAL.Context;

namespace ShopDAL.Repository
{
    public class CategoryRepository : IRepository<Category>, ICategory<Category>
    {
        //Instantiate a new list of categories
        private List<Category> Categories = new List<Category>();
        //Instantiate a new list of products
        private List<Product> Products = new List<Product>();
        public void Add(Category cat)
        {
            using (var ctx = new DbConn())
            {

                cat.CategoryName = cat.CategoryName.Replace(' ', '-');

                ctx.categories.Add(cat);
                ctx.SaveChanges();
            }
        }

        public void AddWithId(int id, Category cat)
        {
            using (var ctx = new DbConn())
            {
                cat.ChildOf = id;
                ctx.categories.Add(cat);
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            //Finds category by id
            Category cat = Find(id);
            using (var ctx = new DbConn())
            {
                //Attach an object to an existing one in the db, changes made to the object before attach are ignored in the save
                ctx.categories.Attach(cat);
                //Deletes current category
                ctx.categories.Remove(cat);
                ctx.SaveChanges();
            }
        }

        public void Edit(Category cat)
        {
            using (var ctx = new DbConn())
            {
                //Returns the first element of a sequence, or a default value if the sequence contains no elements.
                var catDb = ctx.categories.FirstOrDefault(x => x.id == cat.id);
                catDb.CategoryImage = cat.CategoryImage;
                catDb.CategoryName = cat.CategoryName;
                catDb.ChildOf = cat.ChildOf;
                catDb.CategoryText = cat.CategoryText;
                //catDb.ProductId = cat.ProductId;
                ctx.SaveChanges();

            }
        }

        public Category Find(int? id)
        {
            foreach (var cat in ReadAll())
            {
                if (cat.id == id)
                {
                    return cat;
                }

            }
            return null;
        }
        //Returns a category object with the given name
        public Category FindByName(string categoryName)
        {
            foreach (var cat in ReadAll())
            {
                if (cat.CategoryName == categoryName)
                {
                    return cat;
                }

            }
            return null;
        }
        //Returns a category name 
        public string FindName(string categoryName)
        {
            foreach (var cat in ReadAll())
            {
                if (cat.CategoryName == categoryName)
                {
                    return cat.CategoryName;
                }

            }
            return null;
        }

        public List<Category> GetAllParentCategory()
        {

            List<Category> parents = new List<Category>();

            foreach (var cat in ReadAll())
            {
                if (cat.ChildOf == 0)
                {
                    parents.Add(cat);
                }
            }
            return parents;


        }

        public List<Category> GetParentCategoryById(int id)
        {
            List<Category> parents = new List<Category>();

            foreach (var cat in ReadAll())
            {
                if (cat.id == cat.ChildOf && cat.ChildOf == id)
                {
                    parents.Add(cat);
                }
            }
            return parents;
        }

        public List<Category> GetChilds()
        {
            List<Category> childs = new List<Category>();

            foreach (var cat in ReadAll())
            {
                if (cat.ChildOf > 0)
                {
                    childs.Add(cat);
                }
            }
            return childs;

        }

        public List<Category> GetSubCategories(int id)
        {

            List<Category> childs = new List<Category>();

            foreach (var cat in ReadAll())
            {
                if (cat.ChildOf == id)
                {
                    childs.Add(cat);
                }
            }
            return childs;
        }

        public Category Read(int id)
        {
            using (var ctx = new DbConn())
            {
                return ctx.categories.Include(x => x.Products).FirstOrDefault(x => x.id == id);
            }
        }

        //Returns all categories
        public List<Category> ReadAll()
        {
            using (var ctx = new DbConn())
            {
                return ctx.categories.Include("Products").ToList();
            }
        }
    }
}
