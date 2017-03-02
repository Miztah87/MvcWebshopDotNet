using ShopDAL.Context;
using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDAL.Repository
{
    public class CheckoutRepository
    {
        public void AddOrder(Order order)
        {
            using (var ctx = new DbConn())
            {
                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }

        public bool CheckIfComplete(int id, string userName)
        {
            using (var ctx = new DbConn())
            {
                bool isValid = ctx.Orders.Any(
                  o => o.OrderId == id &&
                  o.Username == userName);

                return isValid;
            }
        }

        public List<Order> GetCurrentOrder(int id)
        {
            using (var ctx = new DbConn())
            {
                var orders = ctx.Orders.Include("OrderDetails.Product").Where(c => c.OrderId == id).ToList();

                return orders;
            }
        }
    }
}
