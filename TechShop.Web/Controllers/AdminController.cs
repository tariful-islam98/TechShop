using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.Entities.Services;
using TechShop.Services;
using TechShop.Web.ViewModels;

namespace TechShop.Web.Controllers
{
    public class AdminController : Controller
    {
        public static int adminId = 0;

        // GET: Admin
        public ActionResult Dashboard()
        {
            if (Session["AdminEmail"] != null)
            {
                DashboardViewModels models = new DashboardViewModels();
                int month = DateTime.Now.Month;
                models.OrderCountMonth = DashboardServices.Instance.OrderCount(null, month);
                models.PendingOrderMonth = DashboardServices.Instance.OrderCount("Pending", month);
                models.PaymentRecievedMonth = DashboardServices.Instance.OrderCount("Payment Recieved", month);

                models.OrderCountTotal = DashboardServices.Instance.OrderCount(null, null);
                models.PendingOrderTotal = DashboardServices.Instance.OrderCount("Pending", null);
                models.PaymentRecievedTotal = DashboardServices.Instance.OrderCount("Payment Recieved", null);

                models.TotalProducts = DashboardServices.Instance.ProductCount(null);
                models.OutOfStock = DashboardServices.Instance.ProductCount("stock");
                models.TotalPayment = DashboardServices.Instance.TatalAmount();

                return View(models);
            }
            else
            {
                return RedirectToAction(nameof(AdminLogin));
            }
        }

        #region Register
        //Admin Register
        [HttpGet]
        public ActionResult AdminRegister()
        {
            if (Session["AdminEmail"] != null)
            {
                AdminRegisterViewModel model = new AdminRegisterViewModel();
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(AdminLogin));
            }
        }

        [HttpPost]
        public ActionResult AdminRegister(AdminRegisterViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                if (ModelState.IsValid)
                {
                    var admin = new Admin();
                    admin.Name = model.Name;
                    admin.Address = model.Address;
                    admin.Email = model.Email;
                    admin.Password = model.Password;

                    AdminAccountServices.Instance.RegisterAdmin(admin);

                    return RedirectToAction(nameof(Dashboard));
                }
                else
                {
                    return new HttpStatusCodeResult(500);
                }
            }
            else
            {
                return RedirectToAction(nameof(AdminLogin));
            }
        }
        #endregion


        #region AdminLogin
        [HttpGet]
        public ActionResult AdminLogin()
        {
            if (Session["AdminEmail"] != null)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminLoginViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            else
            {
                var admin = AdminAccountServices.Instance.GetLoginAccount(model.Email, model.Password);
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                else
                {
                    if (admin == null)
                    {
                        ModelState.AddModelError("AdminLoginError", "Invalid Id or Password");
                        return View(model);
                    }
                    else
                    {
                        Session["AdminEmail"] = admin.Email;
                        Session["AdminName"] = admin.Name;
                        adminId = admin.ID;
                        return RedirectToAction(nameof(Dashboard));
                    }
                }
            }

        }
        #endregion

        #region AdminAccount
        [HttpGet]
        public ActionResult AdminAccount()
        {
            if (Session["AdminEmail"] != null)
            {
                AdminAccountViewModel model = new AdminAccountViewModel();
                var admin = AdminAccountServices.Instance.GetAccount(adminId);
                model.ID = admin.ID;
                model.Name = admin.Name;
                model.Address = admin.Address;
                model.Email = admin.Email;

                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(AdminLogin));
            }
        }
        #endregion


        #region Updation

        [HttpGet]
        public ActionResult Update(int ID)
        {
            if (Session["AdminEmail"] != null)
            {
                AdminEditViewModel model = new AdminEditViewModel();

                var admin = AdminAccountServices.Instance.GetAccount(ID);

                model.ID = admin.ID;
                model.Name = admin.Name;
                model.Address = admin.Address;
                model.Email = admin.Email;
                model.Password = admin.Password;

                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(AdminLogin));
            }
        }

        [HttpPost]
        public ActionResult Update(AdminEditViewModel model)
        {
            if (Session["AdminEmail"] != null)
            {
                var admin = AdminAccountServices.Instance.GetAccount(model.ID);
                admin.Name = model.Name;
                admin.Address = model.Address;
                admin.Email = model.Email;
                admin.Password = model.Password;

                AdminAccountServices.Instance.UpdateAccount(admin);

                return RedirectToAction(nameof(Dashboard));
            }
            else
            {
                return RedirectToAction(nameof(AdminLogin));
            }
        }

        #endregion

        #region AdminLogout
        public ActionResult AdminLogout()
        {
            Session["AdminEmail"] = null;
            adminId = 0;
            return RedirectToAction(nameof(AdminLogin));
        }
        #endregion

    }

    //.... https://docs.microsoft.com/en-us/aspnet/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/shopping-cart
}