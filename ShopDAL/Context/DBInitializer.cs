using ShopDAL.DomainModel;
using System.Collections.Generic;
using System.Data.Entity;

namespace ShopDAL.Context
{
    class DbInitializer : DropCreateDatabaseAlways<DbConn>

    {
        protected override void Seed(DbConn context)
        {

            //New list of categories
            IList<Category> cat = new List<Category>();
            //New user

            //New products
            Product prod1 = context.products.Add(new Product() { id = 1, Type = "Bolt", Description = "parts for lift", Price = 54 });
            Product prod2 = context.products.Add(new Product() { id = 2, Type = "Wrench", Description = "tool for cars", Price = 85 });
            Product prod3 = context.products.Add(new Product() { id = 3, Type = "Joint", Description = "parts for lift", Price = 54 });
            Product prod4 = context.products.Add(new Product() { id = 4, Type = "lamp", Description = "parts for vehicles", Price = 134 });

            //Adding products to categories
            cat.Add(new Category()
            {
                id = 1,
                CategoryName = "SpareParts",
                ChildOf = 0,
                Products = new List<Product>() { prod1, prod3 }
            });

            cat.Add(new Category()
            {
                id = 2,
                CategoryName = "Tools",
                ChildOf = 0,
                Products = new List<Product>() { prod2 }


            });

            cat.Add(new Category()
            {
                id = 3,
                CategoryName = "Vehicles",
                ChildOf = 2,
                Products = new List<Product>() { prod4 }


            });


            //Add all categories to the list of category
            foreach (Category categories in cat)
            {
                context.categories.Add(categories);
            }

            //Seeding all data with context
            base.Seed(context);
        }
    }
}
