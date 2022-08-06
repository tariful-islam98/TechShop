using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechShop.Entities.Services;

namespace TechShop.Web.ViewModels
{
    public class ProductsWidgetViewModel
    {
        public List<Product> Products { get; set; }
        public bool IsLatestProducts { get; set; }
    }
}