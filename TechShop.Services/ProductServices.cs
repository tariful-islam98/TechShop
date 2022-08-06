using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Database;
using TechShop.Entities.Services;
using System.Data.Entity;
using TechShop.Entities.Code;

namespace TechShop.Services
{
    public class ProductServices
    {
        #region Singleton
        public static ProductServices Instance
        {
            get
            {
                if (instance == null) instance = new ProductServices();

                return instance;
            }
        }
        private static ProductServices instance { get; set; }

        private ProductServices()
        {
        }

        #endregion

        public int GetMaximumPrice()
        {
            using (var contex = new TSContext())
            {
                return (int)(contex.Products.Max(x => x.Price));
            }
        }

        public List<Product> SearchProducts(string searchTerm, int? minPrice, int? maxPrice, int? categoryID, int? sortBy, int pageNo, int pageSize)
        {
            using (var context = new TSContext())
            {
                var products = context.Products.ToList();

                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.ID == categoryID.Value).ToList();
                }
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }
                if (minPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minPrice.Value).ToList();
                }
                if (maxPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maxPrice.Value).ToList();
                }
                if (sortBy.HasValue)
                {
                    var sort = (SortByEnums)sortBy.Value;

                    switch (sort)
                    {
                        case SortByEnums.Default:
                            products = products.ToList();
                            break;
                        case SortByEnums.Popularity:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case SortByEnums.Latest:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case SortByEnums.PriceLowToHigh:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case SortByEnums.PriceHighToLow:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:
                            products = products.ToList();
                            break;
                    }
                }

                return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public int SearchProductsCount(string searchTerm, int? minPrice, int? maxPrice, int? categoryID, int? sortBy)
        {
            using (var context = new TSContext())
            {
                var products = context.Products.ToList();

                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.Category.ID == categoryID.Value).ToList();
                }
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }
                if (minPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minPrice.Value).ToList();
                }
                if (maxPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maxPrice.Value).ToList();
                }
                if (sortBy.HasValue)
                {
                    var sort = (SortByEnums)sortBy.Value;

                    switch (sort)
                    {
                        case SortByEnums.Default:
                            products = products.ToList();
                            break;
                        case SortByEnums.Popularity:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case SortByEnums.Latest:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case SortByEnums.PriceLowToHigh:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case SortByEnums.PriceHighToLow:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                        default:
                            products = products.ToList();
                            break;
                    }
                }

                return products.Count;
            }
        }

        public void SaveProduct(Product product)
        {
            using (var context = new TSContext())
            {
                context.Entry(product.Category).State = System.Data.Entity.EntityState.Unchanged;

                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var context = new TSContext())
            {
                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int ID)
        {
            using (var context = new TSContext())
            {
                var product = context.Products.Find(ID);
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }

        public Product GetProduct(int ID)
        {
            using (var context = new TSContext())
            {
                return context.Products.Where(x => x.ID == ID).Include(x => x.Category).FirstOrDefault();
            }
        }
        public List<Product> GetAllProducts()
        {
            using (var context = new TSContext())
            {
                return context.Products.Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProducts(List<int> IDs)
        {
            using (var context = new TSContext())
            {
                return context.Products.Where(product => IDs.Contains(product.ID)).ToList();
            }
        }

        public List<Product> GetProducts(int pageNo)
        {
            int pageSize = 10;// int.Parse(ConfigurationsService.Instance.GetConfig("ListingPageSize").Value);

            using (var context = new TSContext())
            {
                return context.Products.OrderBy(x => x.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();

                //return context.Products.Include(x => x.Category).ToList();
            }
        }
 
        public List<Product> GetProducts(int pageNo, int pageSize)
        {
            using (var context = new TSContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProducts(string search, int pageNo, int pageSize)
        {
            using (var context = new TSContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(product => product.Name != null &&
                         product.Name.ToLower().Contains(search.ToLower()))
                         .OrderBy(x => x.ID)
                         .Skip((pageNo - 1) * pageSize)
                         .Take(pageSize)
                         .Include(x => x.Category)
                         .ToList();
                }
                else
                {
                    return context.Products
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Category)
                        .ToList();
                }
            }
        }
       
        public List<Product> GetLatestProducts(int noOfProducts)
        {
            using (var context = new TSContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Take(noOfProducts).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProductsByCategory(int categoryID, int pageSize)
        {
            using (var context = new TSContext())
            {
                return context.Products.Where(x => x.Category.ID == categoryID).OrderByDescending(x => x.ID).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public int GetProductsCount(string search)
        {
            using (var context = new TSContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(product => product.Name != null &&
                         product.Name.ToLower().Contains(search.ToLower()))
                         .Count();
                }
                else
                {
                    return context.Products.Count();
                }
            }
        }
    }
}
