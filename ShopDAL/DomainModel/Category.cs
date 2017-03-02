using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ShopDAL.DomainModel
{
    public class Category
    {
     
        [Key]
        public int id { get; set; }
        public string CategoryName { get; set; }
        //[RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string CategoryImage { get; set; }
        [DefaultValue(0)]
        public int ChildOf { get; set; }
        [AllowHtml]
        public string CategoryText { get; set; }
        public string CategorySection { get; set; }
        public List<Product> Products { get; set; }
    }
}
