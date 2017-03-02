using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebshopMvc.Models.ViewModels
{
    public class ProductCatalog
    {
        // GET: ProductCatalog
        public int? SelectedCategoryId { get; set; }
        public int? SelectedSubCategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public SelectList listOfCategories { get; set; }
        public IEnumerable<Category> SubCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public List<List<Category>> ListsOfCat { get; set; }
    }
}