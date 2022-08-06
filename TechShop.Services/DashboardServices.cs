using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Database;

namespace TechShop.Services
{
    public class DashboardServices
    {
        #region Singleton
        public static DashboardServices Instance
        {
            get
            {
                if (instance == null) instance = new DashboardServices();

                return instance;
            }
        }
        private static DashboardServices instance { get; set; }

        private DashboardServices()
        {
        }

        #endregion

        //public int OrderCountMonth(int month)
        //{
        //    using (var context = new TSContext())
        //    {
        //        return context.Orders.Where(x => x.OrderedAt.Month == month).Count();
        //    }
        //}
        public int OrderCount( string status, int? month)
        {
            using (var context = new TSContext())
            {
                var order = context.Orders.Count();

                if (month.HasValue && string.IsNullOrEmpty(status))
                {
                    order = context.Orders.Where(x => x.OrderedAt.Month == month).Count();
                }
                else if (!string.IsNullOrEmpty(status) && !month.HasValue)
                {
                    order = context.Orders.Where(x => x.Status.Equals(status)).Count();
                }
                else if (month.HasValue && !string.IsNullOrEmpty(status))
                {
                    order = context.Orders.Where(x => x.OrderedAt.Month == month && x.Status.Equals(status)).Count();
                }
                else
                {
                    order = context.Orders.Count();
                }
                return order;
            }
        }

        public int ProductCount(string param)
        {
            using (var context = new TSContext())
            {
                int data = 0;
                if (!string.IsNullOrEmpty(param) && param.Equals("stock"))
                {
                    data = context.Products.Where(x => x.Quantity <= 0).Count();
                }
                else
                {
                    data = context.Products.Count();
                }
                return data;
            }
        }
        
        public int TatalAmount()
        {
            using (var context = new TSContext())
            {
                var amount = (int) context.Orders.Sum(x => x.TotalAmount);

                return amount;
            }
        }

    }
}
