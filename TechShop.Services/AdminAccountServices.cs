using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Database;
using TechShop.Entities.Services;

namespace TechShop.Services
{
    public class AdminAccountServices
    {
        #region Singleton

        public static AdminAccountServices Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AdminAccountServices();
                }
                return instance;
            }
        }
        private static AdminAccountServices instance { get; set; }

        private AdminAccountServices() { }
        #endregion

        #region Register
        public void RegisterAdmin(Admin admin)
        {
            using (var context = new TSContext())
            {
                context.Admins.Add(admin);
                context.SaveChanges();
            }
        }
        #endregion

        #region AdminLogin
        public Admin GetLoginAccount(string email, string password)
        {
            using (var context = new TSContext())
            {
                return context.Admins.Where(x => x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();
            }
        }
        #endregion

        #region GetAccountById
        public Admin GetAccount(int Id)
        {
            using (var context = new TSContext())
            {
                return context.Admins.Where(x => x.ID == Id).FirstOrDefault();
            }
        }
        #endregion

        #region GetAllAccounts
        public List<Admin> GetAllAccounts()
        {
            using (var context = new TSContext())
            {
                return context.Admins.ToList();
            }
        }
        #endregion

        #region Update
        public void UpdateAccount(Admin admin)
        {
            using (var context = new TSContext())
            {
                context.SaveChanges();
            }
        }
        #endregion

        #region RemoveAccount
        public void RemoveAccount(int Id)
        {
            using (var context = new TSContext())
            {
                var admin = context.Admins.Find(Id);
                context.Admins.Remove(admin);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
