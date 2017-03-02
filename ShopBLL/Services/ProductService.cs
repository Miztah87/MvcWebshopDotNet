using ShopDAL;
using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBLL.Services
{
    public class ProductService
    {
        private FacadeDAL facade = new FacadeDAL();
        public IEnumerable<Product> ReadAll()
        {
            List<Product> products = facade.GetProductRepository().ReadAll();
            return products;
        }

        public void AddProduct(Product product)
        {
            facade.GetProductRepository().Add(product);
        }

        public Product GetDetails(int id)
        {
            Product product = facade.GetProductRepository().Find(id);

            return product;
        }

        public string FindName(string productName)
        {
            return facade.GetProductRepository().FindName(productName);
        }

        public Product Find(int? id)
        {
            return facade.GetProductRepository().Find(id);
        }

        public void Update(int id)
        {
            Product product = new Product();
            product.id = id;
            facade.GetProductRepository().Edit(product);

        }

        public void Delete(int id)
        {
            facade.GetProductRepository().Delete(id);
        }
    }
}
