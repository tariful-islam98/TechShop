using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechShop.Entities.Services;

namespace TechShop.Web.ViewModels
{
    public class HomeViewModels
    {
        public List<Category> FeaturedCategories { get; set; }
        public List<Product> FeaturedProducts { get; set; }
    }
}