using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.Services;
using TechShop.Web.ViewModels;

namespace TechShop.Web.Controllers
{
    public class WidgetController : Controller
    {
        // GET: Widget
        public ActionResult ProductsWidget(int? CategoryID = 0)
        {
            ProductsWidgetViewModel model = new ProductsWidgetViewModel();

            if (CategoryID.HasValue && CategoryID.Value > 0)
            {
                model.Products = ProductServices.Instance.GetProductsByCategory(CategoryID.Value, 4);
            }
            else
            {
                model.Products = ProductServices.Instance.GetProducts(1, 8);
            }
            return PartialView(model);
        }
        
        // GET: Widget
        public ActionResult LatestProductsWidget(bool isLatestProduct)
        {
            ProductsWidgetViewModel model = new ProductsWidgetViewModel();
            model.IsLatestProducts = isLatestProduct;

            if (isLatestProduct)
            {
                model.Products = ProductServices.Instance.GetLatestProducts(4);
            }
            return PartialView(model);
        }
    }
}