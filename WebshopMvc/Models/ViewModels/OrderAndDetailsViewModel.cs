using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebshopMvc.Models.ViewModels
{
    public class OrderAndDetailsViewModel
    {
        public Order order { get; set; }

        public List<OrderDetail> orderDetails { get; set; }
    }
}