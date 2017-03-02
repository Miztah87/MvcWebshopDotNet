using ShopBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebshopMvc.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        //instantiate facade
        private FacadeBLL facade = new FacadeBLL();
        // GET: Product
        public ActionResult Index()
        {
            var products = facade.GetProductService().ReadAll();
            return View(products);
        }

        public ActionResult GetAllProducts()
        {
            var products = facade.GetProductService().ReadAll();
            return View(products);
        }

        public ActionResult GetRandomProducts()
        {

            var products = facade.GetProductService().ReadAll();
            int index = new Random().Next(products.Count());
            var randomProds = facade.GetProductService().ReadAll().Skip(index).Take(5);
            return PartialView(randomProds);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = facade.GetProductService().GetDetails(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Categories = new MultiSelectList(facade.GetCategoryService().ReadAll(), "id", "CategoryName");

            return View();
        }

        // POST: Product/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create([Bind(Include = "id, Position, Type, Description,Quantity,ProductNum,Price,imageUrl, CategoriesId ")] ShopDAL.DomainModel.Product product, int[] CategoriesId)
        {

            if (ModelState.IsValid)
            {
                product.Categories = new List<ShopDAL.DomainModel.Category>();
                foreach (var ids in CategoriesId)
                {
                    product.Categories.Add(new ShopDAL.DomainModel.Category() { id = ids });
                }
                facade.GetProductService().AddProduct(product);

            }

            return Redirect("Index");
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = facade.GetProductService().Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }



        // POST: Product/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id)
        {
            try
            {
                // TODO: Add update logic here
                facade.GetProductService().Update(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var product = facade.GetProductService().Find(id);
            if (product == null)
            {
                return HttpNotFound("Something's missing.....");
            }
            facade.GetProductService().Delete(id);
            return RedirectToAction("Index");
        }
    }
}