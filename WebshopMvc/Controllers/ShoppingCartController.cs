using ShopDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebshopMvc.Models.ViewModels;

namespace WebshopMvc.Controllers
{
    public class ShoppingCartController : Controller
    {
       private FacadeDAL facade = new FacadeDAL();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = facade.GetShoppingCartRepository().GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {

                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }

        // GET: /Store/AddToCart/5
        //[Route("ShoppingCart/AddToCart/{id}/{qty}")]
        //[HttpPost]
        public ActionResult AddToCart(int id)
        {
            //int id = Int32.Parse(Request.QueryString["id"]);
            //int qty = Int32.Parse(Request.QueryString["qty"]);
            //int qantity = Int32.Parse(qty);
            var addedProduct = facade.GetProductRepository().Find(id);

            // Add it to the shopping cart
            var cart = facade.GetShoppingCartRepository().GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = facade.GetShoppingCartRepository().GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string productName = facade.GetProductRepository().Find(id).Type;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = facade.GetShoppingCartRepository().GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cart = facade.GetShoppingCartRepository().GetCart(this.HttpContext);

            // Get the name of the album to display confirmation 
            var product = facade.GetProductRepository().Find(id);
            string productName = product.Type;
            // Update the cart count 
            int itemCount = cart.UpdateCartCount(id, cartCount);

            //Prepare messages
            string msg = "The quantity of " + Server.HtmlEncode(productName) +
                    " has been refreshed in your shopping cart.";
            if (itemCount == 0) msg = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.";
            //
            // Display the confirmation message 
            var results = new ShoppingCartRemoveViewModel
            {
                Message = msg,
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

    }
}