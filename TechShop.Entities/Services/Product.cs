using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Entities.Base;

namespace TechShop.Entities.Services
{
    public class Product : BaseEntity
    {
        public virtual Category Category { get; set; }

        public int CategoryID { get; set; }

        [Range(1, 2000000) ]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int Quantity { get; set; }
    }
}
