using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TechShop.Entities.Services;

namespace TechShop.Web.ViewModels
{
    public class ProductSearchViewModel
    {
        public List<Product> Products { get; set; }
        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }

    public class NewProductViewModel
    {
        [Required]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }


        [MaxLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string ImageUrl { get; set; }

        public List<Category> AvailableCategories { get; set; }
        public int Quantity { get; set; }
    }

    public class EditProductViewModel
    {
        public int ID { get; set; }

        [Required]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }


        [MaxLength(500)]
        public string Description { get; set; }

        [Range(1, 2000000)]
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string ImageUrl { get; set; }

        public List<Category> AvailableCategories { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductViewModel
    {
        public Product Product { get; set; }
    }
}