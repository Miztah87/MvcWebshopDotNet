using ShopDAL.Context;
using ShopDAL.DomainModel;
using ShopDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDAL.Repository
{
    class ProductRepository : IRepository<Product>
    {
        //Instantiate a new list of products
        private List<Product> products = new List<Product>();
        public void Add(Product prod)
        {
            using (var ctx = new DbConn())
            {
                foreach (var cat in prod.Categories)
                {
                    ctx.Entry(cat).State = EntityState.Unchanged;
                }
                ctx.products.Add(prod);
                ctx.SaveChanges();
            }
        }



        public void Delete(int id)
        {
            //Finds product by id
            Product prod = Find(id);
            using (var ctx = new DbConn())
            {
                //Attach an object to an existing one in the db, changes made to the object before attach are ignored in the save
                ctx.products.Attach(prod);
                //Deletes current category
                ctx.products.Remove(prod);
                ctx.SaveChanges();
            }
        }


        public void Edit(Product prod)
        {
            using (var ctx = new DbConn())
            {
                //Returns the first element of a sequence, or a default value if the sequence contains no elements.
                var prodDb = ctx.products.FirstOrDefault(x => x.id == prod.id);

                //prodDb.CategoryId = prod.CategoryId;
                prodDb.Position = prod.Position;
                prodDb.Type = prod.Type;
                prodDb.Description = prod.Description;
                prodDb.Quantity = prod.Quantity;
                prodDb.ProductNum = prod.ProductNum;
                prodDb.Price = prod.Price;
                ctx.SaveChanges();

            }
        }
        //Returns product with the given id
        public Product Find(int? id)
        {
            foreach (var prod in ReadAll())
            {
                if (prod.id == id)
                {
                    return prod;
                }

            }
            return null;
        }
        //Returns a product object with the given type
        public Product FindByName(string productType)
        {
            foreach (var prod in ReadAll())
            {
                if (prod.Type == productType)
                {
                    return prod;
                }

            }
            return null;
        }
        //Returns a product type 
        public string FindName(string productType)
        {
            foreach (var prod in ReadAll())
            {
                if (prod.Type == productType)
                {
                    return prod.Type;
                }

            }
            return null;
        }
        //Returns all products
        public List<Product> ReadAll()
        {
            using (var ctx = new DbConn())
            {
                return ctx.products.ToList();
            }
        }
    }
}
