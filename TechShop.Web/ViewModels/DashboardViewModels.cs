using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechShop.Web.ViewModels
{
    public class DashboardViewModels
    {
        public int OrderCountMonth { get; set; }
        public int PendingOrderMonth { get; set; }
        public int PaymentRecievedMonth { get; set; }

        public int OrderCountTotal { get; set; }
        public int PendingOrderTotal { get; set; }
        public int PaymentRecievedTotal { get; set; }

        public int TotalProducts { get; set; }
        public int OutOfStock { get; set; }
        public int TotalPayment { get; set; }
    }
}