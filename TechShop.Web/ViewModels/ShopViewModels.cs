using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechShop.Entities.Services;
using TechShop.Web.Models;

namespace TechShop.Web.ViewModels
{
    public class CheckoutViewModel
    {
        public List<Product> CartProducts { get; set; }
        public List<int> CartProductIDs { get; set; }
        public int PaymentID { get; set; }
        public string PaymentMethod { get; set; }

        public ApplicationUser User { get; set; }
    }

    public class PaymentMethodViewModel
    {
        //public int OrderID { get; set; }
        public int PaymentID { get; set; }
    }

    public class ShopViewModel
    {
        public int MaxPrice { get; set; }
        public List<Category> Categories { get; set; }
        public List<Category> FeaturedCategories { get; set; }
        public List<Product> Products { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }

        public Pager Pager { get; set; }
        public string SearchTerm { get; set; }
    }

    public class FilterProductsViewModel
    {
        public List<Product> Products { get; set; }

        public Pager Pager { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }
    }
}