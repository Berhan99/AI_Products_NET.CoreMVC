using DataAccessLayer.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(ETicaretUygulamasiContext ctx) : base(ctx)
        {

        }
        private ETicaretUygulamasiContext Context
        {
            get
            {
                return genericContext as ETicaretUygulamasiContext;
            }
        }
        public void DeleteFromCategory(int productId, int categoryId)
        {
            var cmd = "delete from productcategory where ProductId=@p0 and CategoryId=@p1";
            Context.Database.ExecuteSqlRaw(cmd, productId, categoryId);

        }

        public Category GetByIdWithproducts(int categoryId)
        {

            return Context.Categories.Where(x => x.CategoryId == categoryId)
                 .Include(y => y.ProductCategories)
                 .ThenInclude(z => z.Product)
                 .FirstOrDefault();

        }
    }
}
