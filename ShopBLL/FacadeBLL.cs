using ShopBLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBLL
{
    public class FacadeBLL
    {
        public CategoryService GetCategoryService()
        {
            return new CategoryService();
        }

        public ProductService GetProductService()
        {
            return new ProductService();
        }        

    }
}
