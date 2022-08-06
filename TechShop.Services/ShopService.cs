using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Database;
using TechShop.Entities.Services;

namespace TechShop.Services
{
    public class ShopService
    {
        #region Singleton
        public static ShopService Instance
        {
            get
            {
                if (instance == null) instance = new ShopService();

                return instance;
            }
        }
        private static ShopService instance { get; set; }

        private ShopService()
        {
        }

        #endregion

        public int SaveOrder(Order order)
        {
            using (var context = new TSContext())
            {
                context.Orders.Add(order);
                return context.SaveChanges();
            }
        }

        public int LatestOrderId()
        {
            using (var context = new TSContext())
            {
                var id = context.Orders.OrderByDescending(x => x.ID).Select(x => x.ID).Take(1).FirstOrDefault();
                return (int) id;
            }
        }
    }
}
