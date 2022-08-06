using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class ShopController : Controller
    {
        //ProductServices productServices = new ProductServices();
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

        public ActionResult Index(string searchTerm, int? minPrice, int? maxPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.ShopPageSize();
            ShopViewModel model = new ShopViewModel();

            model.SearchTerm = searchTerm;
            model.Categories = CategoryServices.Instance.GetAllCategories();
            model.FeaturedCategories = CategoryServices.Instance.GetFeaturedCategories();
            model.MaxPrice = ProductServices.Instance.GetMaximumPrice();
            model.SortBy = sortBy;

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            int totalCount = ProductServices.Instance.SearchProductsCount(searchTerm, minPrice, maxPrice, categoryID, sortBy);
            model.Products = ProductServices.Instance.SearchProducts(searchTerm, minPrice, maxPrice, categoryID, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return View(model);
        }

        public ActionResult FilterProducts(string searchTerm, int? minPrice, int? maxPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.ShopPageSize();
            FilterProductsViewModel model = new FilterProductsViewModel();

            model.SearchTerm = searchTerm;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.CategoryID = categoryID;
            model.SortBy = sortBy;

            int totalCount = ProductServices.Instance.SearchProductsCount(searchTerm, minPrice, maxPrice, categoryID, sortBy);
            model.Products = ProductServices.Instance.SearchProducts(searchTerm, minPrice, maxPrice, categoryID, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return PartialView(model);
        }

        public static int RanPaymentID = 0;

        [Authorize]
        public ActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();

            var CartProductsCookie = Request.Cookies["CartProducts"];

            if (CartProductsCookie != null && !string.IsNullOrEmpty(CartProductsCookie.Value))
            {
                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();

                model.CartProducts = ProductServices.Instance.GetProducts(model.CartProductIDs);

                if (RanPaymentID == 0)
                {
                    model.PaymentID = GeneratePaymentID();
                    RanPaymentID = model.PaymentID;
                }

                model.User = UserManager.FindById(User.Identity.GetUserId());
            }

            return View(model);
        }

        public ActionResult Payment()
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel();
            model.PaymentID = RanPaymentID;
            return PartialView(model);
        }

        //productIDs should beformatted like = "7-7-9-1"
        public JsonResult PlaceOrder(string productIDs, int paymentId, string paymentMethod)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!string.IsNullOrEmpty(productIDs))
            {
                var productQuantities = productIDs.Split('-').Select(x => int.Parse(x)).ToList();

                var boughtProducts = ProductServices.Instance.GetProducts(productQuantities.Distinct().ToList());

                Order newOrder = new Order();
                newOrder.UserID = User.Identity.GetUserId();
                newOrder.UserEmail = User.Identity.GetUserName();
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                newOrder.PaymentID = paymentId;
                newOrder.PaymentMethod = paymentMethod;
                newOrder.TotalAmount = boughtProducts.Sum(x => x.Price * productQuantities.Where(productID => productID == x.ID).Count());

                newOrder.OrderItems = new List<OrderItem>();
                newOrder.OrderItems.AddRange(boughtProducts.Select(x => new OrderItem() { ProductID = x.ID, Quantity = productQuantities.Where(productID => productID == x.ID).Count() }));

                bool isInStock = true;
                foreach (var item in productQuantities.Distinct())
                {
                    var quantity = productQuantities.Count(x => x == item);
                    var product = ProductServices.Instance.GetProduct(item);
                    product.Quantity = product.Quantity - quantity;

                    if (product.Quantity >= 0)
                    {
                        ProductServices.Instance.UpdateProduct(product);
                        isInStock = true;
                    }
                    else
                    {
                        result.Data = new { Success = false };
                        isInStock = false;
                    }
                }

                if (isInStock != false)
                {
                    var rowsEffected = ShopService.Instance.SaveOrder(newOrder);
                    RanPaymentID = 0;
                    result.Data = new { Success = true, Rows = rowsEffected };
                }
                else
                {
                    result.Data = new { Success = false };
                }

            }
            else
            {
                result.Data = new { Success = false };
            }

            return result;
        }

        public int GeneratePaymentID()
        {
            var random = new Random();
            int paymentID = random.Next(100001, 999999);

            return paymentID;
        }
    }
}