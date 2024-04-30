using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService: IValidator<Product>
    {
        Task<Product> GetById(int Id);
        Product GetByIdWithCategories(int Id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name, int page ,int pageSize);
        Task<List<Product>> GetAll();
        bool Create(Product entity);
        Task<Product> CreateAsync(Product entity);
        void Update(Product entity);
        bool Update(Product entity,int[] categoryIds);
        void Delete(Product entity);
        Task DeleteAsync(Product entity);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchFilter);
        Task UpdateAsync(Product entityFromDb, Product product);
    }
}
