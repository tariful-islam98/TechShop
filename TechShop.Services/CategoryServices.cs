using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Database;
using TechShop.Entities.Services;
using System.Data.Entity;

namespace TechShop.Services
{
    public class CategoryServices
    {
        #region Singleton
        public static CategoryServices Instance
        {
            get
            {
                if (instance == null) instance = new CategoryServices();

                return instance;
            }
        }
        private static CategoryServices instance { get; set; }
        private CategoryServices()
        {
        }
        #endregion

        public void SaveCategory(Category category)
        {
            using (var context = new TSContext()) 
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }
        
        public void UpdateCategory(Category category)
        {
            using (var context = new TSContext()) 
            {
                context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(int ID)
        {
            using (var context = new TSContext())
            {
                var category = context.Categories.Where(x => x.ID == ID).Include(x => x.Products).FirstOrDefault();

                context.Products.RemoveRange(category.Products); //first delete products of this category
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }
        
        public List<Category> GetAllCategories()
        {
            using (var context = new TSContext()) 
            {
                return context.Categories.Include(x => x.Products).ToList();
            }
        }
        
        public List<Category> GetFeaturedCategories()
        {
            using (var context = new TSContext()) 
            {
                return context.Categories.Where(x=> x.IsFeatured && x.ImageUrl!=null).ToList();
            }
        }
        
        public Category GetCategory(int ID)
        {
            using (var context = new TSContext()) 
            {
                return context.Categories.Find(ID);
            }
        }
        
        public Category GetCategoryByName(string Name)
        {
            using (var context = new TSContext()) 
            {
                return context.Categories.Where(x=> x.Name.ToLower().Equals(Name.ToLower())).FirstOrDefault();
            }
        }
        
        public int GetCategoriesCount(string search)
        {
            using (var context = new TSContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Categories.Where(category => category.Name != null &&
                         category.Name.ToLower().Contains(search.ToLower())).Count();
                }
                else
                {
                    return context.Categories.Count();
                }
            }
        }

        public List<Category> GetCategories(string search, int pageNo)
        {
            int pageSize = 10;

            using (var context = new TSContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Categories.Where(category => category.Name != null &&
                         category.Name.ToLower().Contains(search.ToLower()))
                         .OrderBy(x => x.ID)
                         .Skip((pageNo - 1) * pageSize)
                         .Take(pageSize)
                         .Include(x => x.Products)
                         .ToList();
                }
                else
                {
                    return context.Categories
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Products)
                        .ToList();
                }
            }
        }        
    }
}
