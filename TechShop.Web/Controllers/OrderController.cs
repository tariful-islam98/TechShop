using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechShop.Entities.Services;
using TechShop.Services;
using TechShop.Web.Models;
using TechShop.Web.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TechShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public static bool isMailSent = false;

        // GET: Order
        public ActionResult Index(string userID, string userEmail, string status)
        {
            if (Session["AdminEmail"] != null)
            {
                OrdersViewModel model = new OrdersViewModel();
                model.UserID = userID;
                model.UserEmail = userEmail;
                model.Status = status;

                model.Orders = OrdersService.Instance.SearchOrders(userID, userEmail, status);
                var totalRecords = OrdersService.Instance.SearchOrdersCount(userID, userEmail, status);

                //model.Pager = new Pager(totalRecords, pageNo, pageSize);

                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public ActionResult Details(int ID)
        {
            if (Session["AdminEmail"] != null)
            {
                OrderDetailsViewModel model = new OrderDetailsViewModel();

                model.Order = OrdersService.Instance.GetOrderByID(ID);

                if (model.Order != null)
                {
                    model.OrderBy = UserManager.FindById(model.Order.UserID);
                }

                model.AvailableStatuses = new List<string>() { "Pending", "Payment Recieved", "Confirmed", "Delivered" };

                return View(model);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        //#region Invoice
        //public 
        //#endregion

        // Change Order Status...
        public async Task<JsonResult> ChangeStatus(string status, int ID)
        {
            var order = OrdersService.Instance.GetOrderByID(ID);
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            var isMailed = false;

            if (status.Equals("Payment Recieved") && order.IsMailed == false)
            {
                await SendEmail(ID);
                isMailed = isMailSent;
            }

            result.Data = new { Success = OrdersService.Instance.UpdateOrderStatus(ID, status, isMailed) };
            isMailSent = false;
            return result;
        }

        #region POST: /SendEmail  

        /// <summary>  
        /// <returns>Return - Response information</returns>  
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendEmail(int OrderId)
        {
            try
            {
                // Verification  
                if (ModelState.IsValid)
                {
                    OrderDetailsViewModel model = new OrderDetailsViewModel();

                    model.Order = OrdersService.Instance.GetOrderByID(OrderId);

                    var user1 = UserManager.FindById(model.Order.UserID);
                    string prds = "";
                    foreach (var item in model.Order.OrderItems)
                    {
                        prds = prds+item.Product.Name + " ("+item.Quantity +") - Price:"+ (item.Quantity*item.Product.Price)+"<br />";
                    }
                    // Initialization.
                    string emailMsg = "<h2><b>Dear "+user1.Name+",</b></h2>" +
                        "<font style=\"color: green; font-size: 25px; \">Your payment has been recieved!</font><br /> " +
                        "<font style=\"color: black; font-size: 25px; \"> <b>Payment Id: "+model.Order.PaymentID+", Payment Method: "+model.Order.PaymentMethod+"</b></font><br /><br />" +
                        "<font style=\"color: black; font-size: 22px; \"><u>Order Details</u>:</font><br />" +
                        "<font style=\"color: blue; font-size: 20px; \">Products:</ font ><br /> " +
                        "<font style=\"color: black; font-size: 18px; \">" + prds + "</font><br />" +
                        "<font style=\"color: black; font-size: 25px; \"> <b>Total Price: "+model.Order.TotalAmount+" </b></font><br /><br /> " +
                        "<font style=\"color: black; font-size: 18px; \">Thanks for shopping.With regards,</font><br /> " +
                        "<b style=\"color: blue; font-size: 30px; \">T</b><b style=\"color: red; font-size: 30px; \">Shop</b>";
                    string emailSubject = "Order Confirmation";


                    // Sending Email.  
                    await this.SendEmailAsync(user1.Email, emailMsg, emailSubject);


                    // Info.  
                    return this.Json(new { EnableSuccess = true, SuccessTitle = "Success", SuccessMsg = "Notification has been sent successfully!" });
                }
            }
            catch (Exception ex)
            {
                // Info  
                //Console.Write(ex);

                // Info  
                return this.Json(new { EnableError = true, ErrorTitle = "Error", ErrorMsg = ex.Message });
            }

            // Info  
            return this.Json(new { EnableError = true, ErrorTitle = "Error", ErrorMsg = "Something goes wrong, please try again later" });
        }

        #endregion

        #region Helper  

        #region Send Email method.  

        /// <summary>  
        ///  Send Email method.  
        /// </summary>  
        /// <param name="email">Email parameter</param>  
        /// <param name="msg">Message parameter</param>  
        /// <param name="subject">Subject parameter</param>  
        /// <returns>Return await task</returns>  
        public async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
            // Initialization.  
            bool isSend = false;

            try
            {
                // Initialization.  
                var body = msg;

                string _senderEmail = "******@gmail.com";
                //string _senderPass = "**********";

                MailMessage message = new MailMessage();

                // Settings.  
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(_senderEmail);
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : "Order Confirmation";
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    // Settings.  
                    var credential = new NetworkCredential
                    {
                        UserName = "abir0dhaka@gmail.com",
                        Password = "**********"
                    };

                    // Settings.  
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    //smtp.UseDefaultCredentials = true;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    // Sending  
                    //smtp.Send(message);
                    await smtp.SendMailAsync(message);

                    // Settings.  
                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            isMailSent = isSend;
            
            // info.  
            return isSend;
        }

        #endregion

        #endregion
    }
}
