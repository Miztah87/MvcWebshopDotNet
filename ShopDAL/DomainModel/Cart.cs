using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDAL.DomainModel
{
    public class Cart
    {
        [Key]
        public int id { get; set; }

        public string CartId { get; set; }

        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = " ")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 0 and 100")]
        [DisplayName("Quantity")]
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }
}
