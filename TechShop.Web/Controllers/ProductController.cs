using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.Entities.Services;
using TechShop.Services;
using TechShop.Web.ViewModels;

namespace TechShop.Web.Controllers
{
    public class ProductController : AdminController
    {
        // GET: Product
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

        public ActionResult ProductTable()
        {
            if (Session["AdminEmail"] != null)
            {
                //var pageSize = ConfigurationsService.Instance.PageSize();

                //ProductSearchViewModel model = new ProductSearchViewModel();
                //model.SearchTerm = search;

                //pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

                //var totalRecords = ProductServices.Instance.GetProductsCount(search);
                //model.Products = ProductServices.Instance.GetProducts(search, pageNo.Value, pageSize);

                //model.Pager = new Pager(totalRecords, pageNo, pageSize);
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.Products = ProductServices.Instance.GetAllProducts();

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["AdminEmail"] != null)
            {
                NewProductViewModel model = new NewProductViewModel();

                model.AvailableCategories = CategoryServices.Instance.GetAllCategories();

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult Create(NewProductViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                var newProduct = new Product();
                newProduct.Name = model.Name;
                newProduct.Description = model.Description;
                newProduct.Price = model.Price;
                newProduct.Category = CategoryServices.Instance.GetCategory(model.CategoryID);
                newProduct.ImageUrl = model.ImageUrl;
                newProduct.Quantity = model.Quantity;

                ProductServices.Instance.SaveProduct(newProduct);
                return RedirectToAction("ProductTable");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            if (Session["AdminEmail"] != null)
            {
                EditProductViewModel model = new EditProductViewModel();

                var product = ProductServices.Instance.GetProduct(ID);

                model.ID = product.ID;
                model.Name = product.Name;
                model.Description = product.Description;
                model.Price = product.Price;
                model.CategoryID = product.Category != null ? product.Category.ID : 0;
                model.ImageUrl = product.ImageUrl;
                model.Quantity = product.Quantity;

                model.AvailableCategories = CategoryServices.Instance.GetAllCategories();

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult Edit(EditProductViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                var existingProduct = ProductServices.Instance.GetProduct(model.ID);
                existingProduct.Name = model.Name;
                existingProduct.Description = model.Description;
                existingProduct.Price = model.Price;

                existingProduct.Category = null; //mark it null. Because the referncy key is changed below
                existingProduct.CategoryID = model.CategoryID;
                existingProduct.Quantity = model.Quantity;

                //dont update imageUrl if its empty
                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    existingProduct.ImageUrl = model.ImageUrl;
                }

                ProductServices.Instance.UpdateProduct(existingProduct);

                return RedirectToAction("ProductTable");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }


        //[HttpGet]
        //public ActionResult Delete(int ID)
        //{
        //    var product = productServices.GetProduct(ID);
        //    return View(product);
        //}

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["AdminEmail"] != null)
            {
                ProductServices.Instance.DeleteProduct(id);

                return RedirectToAction("ProductTable");
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpGet]
        public ActionResult Details(int ID)
        {
            ProductViewModel model = new ProductViewModel();

            model.Product = ProductServices.Instance.GetProduct(ID);

            if (model.Product == null) return HttpNotFound();

            return View(model);
        }
    }
}