using ShopDAL.Context;
using ShopDAL.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShopDAL.Repository
{
    public class ShoppingCartRepository
    {
        public const string CartSessionKey = "CartId";
        string ShoppingCartId { get; set; }

        public ShoppingCartRepository GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartRepository();
            cart.ShoppingCartId = cart.GetCartId(context);

            return cart;
        }


        public void AddToCart(Product item)
        {

            using (var ctx = new DbConn())
            {
                // Get the matching cart and album instances
                // var cartItem = Find(item.id);
                var cartItem = ctx.Carts.SingleOrDefault(
               c => c.CartId == ShoppingCartId
               && c.ProductId == item.id);

                if (cartItem == null)
                {

                    cartItem = new Cart
                    {
                        ProductId = item.id,
                        CartId = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now
                    };
                    // Create a new cart item if no cart item exists

                    ctx.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, 
                    // then add one to the quantity
                    cartItem.Count++;
                }
                // Save changes
                ctx.SaveChanges();
            }
        }

        public void AddToCartWithQty(Product item, int qty)
        {

            using (var ctx = new DbConn())
            {
                // Get the matching cart and album instances
                // var cartItem = Find(item.id);
                var cartItem = ctx.Carts.SingleOrDefault(
               c => c.CartId == ShoppingCartId
               && c.ProductId == item.id);

                if (cartItem == null)
                {
                    if (qty != 0)
                    {
                        cartItem = new Cart
                        {
                            ProductId = item.id,
                            CartId = ShoppingCartId,
                            Count = qty,
                            DateCreated = DateTime.Now
                        };
                    }
                    else
                    {
                        throw new HttpException("ex");
                    }

                    // Create a new cart item if no cart item exists

                    ctx.Carts.Add(cartItem);
                }
                else
                {
                    // If the item does exist in the cart, 
                    // then add one to the quantity
                    cartItem.Count++;
                }
                // Save changes
                ctx.SaveChanges();
            }
        }

        public int RemoveFromCart(int id)
        {
            using (var ctx = new DbConn())
            {
                // Get the cart
                var cartItem = ctx.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

                int itemCount = 0;

                if (cartItem != null)
                {
                    //if (cartItem.Count > 1)
                    //{
                    //    cartItem.Count--;
                    //    itemCount = cartItem.Count;
                    //}
                    //else
                    //{
                    ctx.Carts.Remove(cartItem);
                    //}

                    // Save changes
                    ctx.SaveChanges();
                }

                return itemCount;
            }
        }

        public int UpdateCartCount(int id, int cartCount)
        {
            using (var ctx = new DbConn())
            {
                // Get the cart 
                var cartItem = ctx.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

                int itemCount = 0;

                if (cartItem != null)
                {
                    if (cartCount > 0)
                    {
                        cartItem.Count = cartCount;
                        itemCount = cartItem.Count;
                    }
                    else
                    {
                        ctx.Carts.Remove(cartItem);
                    }
                    // Save changes 
                    ctx.SaveChanges();
                }
                return itemCount;
            }
        }

        public void EmptyCart()
        {
            using (var ctx = new DbConn())
            {
                var cartItems = ctx.Carts.Where(cart => cart.CartId == ShoppingCartId);

                foreach (var cartItem in cartItems)
                {
                    ctx.Carts.Remove(cartItem);
                }

                // Save changes
                ctx.SaveChanges();
            }
        }

        public List<Cart> GetCartItems()
        {
            using (var ctx = new DbConn())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                return ctx.Carts.Include("Product").Where(cart => cart.CartId == ShoppingCartId).ToList();
            }

        }

        public int GetCount()
        {
            using (var ctx = new DbConn())
            {
                // Get the count of each item in the cart and sum them up
                int? count = (from cartItems in ctx.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count).Sum();

                // Return 0 if all entries are null
                return count ?? 0;
            }
        }

        public decimal GetTotal()
        {
            using (var ctx = new DbConn())
            {
                //ctx.Configuration.ProxyCreationEnabled = false;
                // Multiply album price by count of that album to get 
                // the current price for each of those albums in the cart
                // sum all album price totals to get the cart total
                decimal? total = (from cartItems in ctx.Carts
                                  where cartItems.CartId == ShoppingCartId
                                  select (int?)cartItems.Count * cartItems.Product.Price).Sum();
                return total ?? decimal.Zero;
            }
        }

        public int CreateOrder(Order order)
        {
            using (var ctx = new DbConn())
            {
                decimal orderTotal = 0;

                var cartItems = GetCartItems();

                // Iterate over the items in the cart, adding the order details for each
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.OrderId,
                        UnitPrice = item.Product.Price,
                        Quantity = item.Count
                    };

                    // Set the order total of the shopping cart
                    orderTotal += (item.Count * item.Product.Price);

                    ctx.OrderDetails.Add(orderDetail);

                }

                // Set the order's total to the orderTotal count
                order.Total = orderTotal;

                // Save the order
                ctx.SaveChanges();

                // Empty the shopping cart
                EmptyCart();

                // Return the OrderId as the confirmation number
                return order.OrderId;
            }
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public Cart Find(int? id)
        {
            foreach (var item in ReadAll())
            {
                if (item.id == id)
                {
                    return item;
                }

            }
            return null;
        }

        public List<Cart> ReadAll()
        {
            using (var ctx = new DbConn())
            {
                return ctx.Carts.ToList();
            }
        }


        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            using (var ctx = new DbConn())
            {
                var shoppingCart = ctx.Carts.Where(c => c.CartId == ShoppingCartId);

                foreach (Cart item in shoppingCart)
                {
                    item.CartId = userName;
                }
                ctx.SaveChanges();
            }

        }
    }
}
