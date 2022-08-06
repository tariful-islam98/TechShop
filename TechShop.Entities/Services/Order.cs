using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Entities.Services
{
    public class Order
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int PaymentID { get; set; }
        public string UserEmail { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime OrderedAt { get; set; }
        public string Status { get; set; }

        public bool IsMailed { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
