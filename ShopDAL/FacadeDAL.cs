using ShopDAL.DomainModel;
using ShopDAL.Repository;
using ShopDAL.Repository.Interfaces;

namespace ShopDAL
{
    public class FacadeDAL
    {

        private IRepository<Category> categoryRepo;
        private ICategory<Category> categoryRepository;
        private IRepository<Product> productRepo;


        public IRepository<Category> GetCategoryRepository()
        {
            if (categoryRepo == null)
            {
                categoryRepo = new CategoryRepository();
            }
            return categoryRepo;
        }

        public ICategory<Category> GetCategoryRepository2()
        {
            if (categoryRepository == null)
            {
                categoryRepository = new CategoryRepository();
            }
            return categoryRepository;
        }

        public IRepository<Product> GetProductRepository()
        {
            if (productRepo == null)
            {
                productRepo = new ProductRepository();
            }
            return productRepo;
        }

        public ShoppingCartRepository GetShoppingCartRepository()
        {
            return new ShoppingCartRepository();
        }

        public CheckoutRepository GetCheckoutRepository()
        {
            return new CheckoutRepository();
        }
    }
}
