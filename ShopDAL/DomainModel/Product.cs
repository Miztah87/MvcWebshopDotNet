using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopDAL.DomainModel
{
    public class Product
    {
       
        [Key]
        public int id { get; set; }
        public int Position { get; set; }
        [DisplayName("Type")]
        public string Type { get; set; }
        public string Description { get; set; }
        [DisplayName("Amount")]
        public int Quantity { get; set; }
        public string ProductNum { get; set; }
        public decimal Price { get; set; }
        [StringLength(1024)]
        public string imageUrl { get; set; }
        public virtual List<Category> Categories { get; set; }
       
    }

}
