using ShopBLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebshopMvc.Controllers
{
    public class CategoryController : Controller
    {   
        //instantiate facade
        private FacadeBLL facade = new FacadeBLL();
        // GET: Category
       
        public ActionResult Index()
        {
            var categories = facade.GetCategoryService().ReadAll();
            return View(categories);
        }

        public ActionResult ListOfCategory()
        {
            var categories = facade.GetCategoryService().ReadAll();

            return PartialView(categories);
        }

        public ActionResult Children()
        {
            var children = facade.GetCategoryService().GetChildren();

            return PartialView(children);
        }

        //[Route("Detail/{pageName}", Name = "ChildDetails", Order = 2)]
        public ActionResult ListOfChilds(string pageName)
        {
            TempData["pageName"] = pageName;
            var children = facade.GetCategoryService().GetParentByPageName(pageName);
            return PartialView(children);
        }


        // GET: Category/Details/5
        [Route("Detail/{pageName}", Name = "CategoryDetails", Order = 2)]

        public ActionResult Details(string pageName)
        {


            TempData["pageName"] = pageName;
            if (pageName == String.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = facade.GetCategoryService().GetDetails(pageName);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        // GET: Category/Create
        [Authorize]
        public ActionResult Create()
        {

            ViewBag.Categories = new SelectList(facade.GetCategoryService().ReadAll(), "id", "CategoryName");

            return View();
        }

        // POST: Category/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create([Bind(Include = "id, CategoryName, ChildOf")] ShopDAL.DomainModel.Category category)
        {

            //Checks if the category name is already exists
            if (category.CategoryName == facade.GetCategoryService().FindName(category.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category already exists");

                return View();
            }
            else
            {

                facade.GetCategoryService().AddCategory(category);

                return Redirect("Index");

            }

        }
        [Authorize]
        public ActionResult CreateChild(int id)
        {

            ViewBag.Categories = new SelectList(facade.GetCategoryService().ReadAll(), "id", "CategoryName");

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateChild(int id, ShopDAL.DomainModel.Category category)
        {


            //Checks if the category name is already exists
            if (category.CategoryName == facade.GetCategoryService().FindName(category.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Category already exists");

                return View();
            }
            else
            {
                facade.GetCategoryService().AddCategoryWithId(id, category);
                return RedirectToAction("Index");

            }
        }

        // GET: Category/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = facade.GetCategoryService().Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, ShopDAL.DomainModel.Category category)
        {

            try
            {

                facade.GetCategoryService().Update(id, category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
        [Authorize]
        [HttpPost]
        public ActionResult Upload(int id, HttpPostedFileBase file, ShopDAL.DomainModel.Category category)
        {


            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/img"));
                    file.SaveAs(path);
                    facade.GetCategoryService().Update(id, category);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Edit", new { id = category.id });
        }

        // GET: Category/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            return View();
        }

        // POST: Category/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var category = facade.GetCategoryService().Find(id);
            if (category == null)
            {
                return HttpNotFound("Something's missing.....");
            }
            facade.GetCategoryService().Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult GetMainNav()
        {
            var listOfNav = facade.GetCategoryService().GetParentCategories();
            return PartialView(@"~/Views/Shared/_MainNavigation.cshtml", listOfNav);
        }

    }
}