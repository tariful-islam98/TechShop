using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TechShop.Entities.Services;

namespace TechShop.Web.ViewModels
{
    public class CategorySearchViewModel
    {
        public List<Category> Categories { get; set; }
        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }

    public class NewCategoryViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public bool IsFeatured { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public bool IsFeatured { get; set; }
    }
}