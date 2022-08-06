using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Database;
using TechShop.Entities.Services;

namespace TechShop.Services
{
    public class OrdersService
    {
        #region Singleton
        public static OrdersService Instance
        {
            get
            {
                if (instance == null) instance = new OrdersService();

                return instance;
            }
        }
        private static OrdersService instance { get; set; }

        private OrdersService()
        {
        }

        #endregion

        public List<Order> SearchOrders(string userID, string userEmail, string status)
        {
            using (var context = new TSContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(userEmail))
                {
                    orders = orders.Where(x => x.UserEmail.ToLower().Contains(userEmail.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.ToList();
            }
        }

        public List<Order> SearchOrdersByUser(string userID)
        {
            using (var context = new TSContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                return orders.ToList();
            }
        }
        public int SearchOrdersCount(string userID, string userEmail, string status)
        {
            using (var context = new TSContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(userEmail))
                {
                    orders = orders.Where(x => x.UserEmail.ToLower().Contains(userEmail.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Count;
            }
        }
                
        public Order GetOrderByID(int ID)
        {
            using (var context = new TSContext())
            {
                return context.Orders.Where(x => x.ID == ID).Include(x => x.OrderItems).Include("OrderItems.Product").FirstOrDefault();
            }
        }

        public bool UpdateOrderStatus(int ID, string status, bool isMailed)
        {
            using (var context = new TSContext())
            {
                var order = context.Orders.Find(ID);

                order.IsMailed = isMailed;
                order.Status = status;

                context.Entry(order).State = EntityState.Modified;

                return context.SaveChanges() > 0;
            }
        }
    }
}
