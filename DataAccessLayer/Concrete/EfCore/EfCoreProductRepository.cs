using DataAccessLayer.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(ETicaretUygulamasiContext ctx) : base(ctx)
        {

        }

        private ETicaretUygulamasiContext Context
        {
            get
            {
                return genericContext as ETicaretUygulamasiContext;
            }
        }

        public List<Product> Get5PopularProducts()
        {
            throw new NotImplementedException();

        }

        public Product GetByIdWithCategories(int Id)
        {

            return Context.Products.Where(x => x.ProductId == Id)
                .Include(y => y.ProductCategories)
                .ThenInclude(z => z.Category)
                .FirstOrDefault();

        }

        public int GetCountByCategory(string category)
        {

            var products = Context.Products.Where(x => x.IsApproved).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Include(x => x.ProductCategories)
                .ThenInclude(y => y.Category)
                .Where(z => z.ProductCategories.Any(a => a.Category.Url == category));
            }

            return products.Count();

        }

        public List<Product> GetHomePageProducts()
        {

            return Context.Products.Where(x => x.IsApproved && x.IsHome).ToList();

        }


        public Product GetProductDetails(string url)
        {

            return Context.Products.Where(x => x.Url == url)
                .Include(y => y.ProductCategories)
                .ThenInclude(z => z.Category)
                .FirstOrDefault();

        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {

            var products = Context.Products.Where(x => x.IsApproved).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Include(x => x.ProductCategories)
                    .ThenInclude(y => y.Category)
                    .Where(z => z.ProductCategories.Any(a => a.Category.Url == name));
            }

            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        }

        public List<Product> GetSearchResult(string searchFilter)
        {

            var products = Context.Products
                .Where(x => x.IsApproved && (x.Name.ToLower().Contains(searchFilter) || x.Description.ToLower().Contains(searchFilter)))
                .AsQueryable();

            return products.ToList();

        }

        public void Update(Product entity, int[] categoryIds)
        {

            var product = Context.Products
                .Include(x => x.ProductCategories)
                .FirstOrDefault(i => i.ProductId == entity.ProductId);

            if (product != null)
            {
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.Description = entity.Description;
                product.Url = entity.Url;
                product.ImageUrl = entity.ImageUrl;
                product.IsApproved = entity.IsApproved;
                product.IsHome = entity.IsHome;
                product.ProductCategories = categoryIds.Select(categoryId => new ProductCategory()
                {
                    ProductId = entity.ProductId,
                    CategoryId = categoryId
                }).ToList();
                
            }

        }
    }
}
