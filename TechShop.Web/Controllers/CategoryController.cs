using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.Entities.Services;
using TechShop.Services;
using TechShop.Web.ViewModels;
using TechShop.Web.Controllers;

namespace TechShop.Web.Controllers
{
    public class CategoryController : AdminController
    {
        // GET: Category
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["AdminEmail"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

        }

        public ActionResult CategoryTable()
        {
            if (Session["AdminEmail"] != null)
            {
                CategorySearchViewModel model = new CategorySearchViewModel();
                //model.SearchTerm = search;

                //pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

                //var totalRecords = CategoryServices.Instance.GetCategoriesCount(search);
                model.Categories = CategoryServices.Instance.GetAllCategories();

                if (model.Categories != null)
                {
                    //model.Pager = new Pager(totalRecords, pageNo, 10);

                    return PartialView("_CategoryTable", model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        #region Creation

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["AdminEmail"] != null)
            {
                NewCategoryViewModel model = new NewCategoryViewModel();

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

        }

        [HttpPost]
        public ActionResult Create(NewCategoryViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                if (ModelState.IsValid)
                {
                    var newCategory = new Category();
                    newCategory.Name = model.Name;
                    newCategory.Description = model.Description;
                    newCategory.ImageUrl = model.ImageUrl;
                    newCategory.IsFeatured = model.IsFeatured;

                    CategoryServices.Instance.SaveCategory(newCategory);

                    return RedirectToAction("CategoryTable");
                }
                else
                {
                    return new HttpStatusCodeResult(500);
                }
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

        }

        #endregion

        #region Updation

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            if (Session["AdminEmail"] != null)
            {
                EditCategoryViewModel model = new EditCategoryViewModel();

                var category = CategoryServices.Instance.GetCategory(ID);

                model.ID = category.ID;
                model.Name = category.Name;
                model.Description = category.Description;
                model.ImageUrl = category.ImageUrl;
                model.IsFeatured = category.IsFeatured;

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                var existingCategory = CategoryServices.Instance.GetCategory(model.ID);
                existingCategory.Name = model.Name;
                existingCategory.Description = model.Description;
                existingCategory.ImageUrl = model.ImageUrl;
                existingCategory.IsFeatured = model.IsFeatured;

                CategoryServices.Instance.UpdateCategory(existingCategory);

                return RedirectToAction("CategoryTable");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }

        }

        #endregion

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            if (Session["AdminEmail"] != null)
            {

                CategoryServices.Instance.DeleteCategory(ID);

                return RedirectToAction("CategoryTable");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }
    }
}