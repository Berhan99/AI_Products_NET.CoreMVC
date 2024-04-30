using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductDetails(string url);
        Product GetByIdWithCategories(int Id);
        List<Product> GetProductsByCategory(string name,int page, int pageSize);
        List<Product> GetSearchResult(string searchFilter);        
        List<Product> GetHomePageProducts();
        int GetCountByCategory(string category);

        void Update(Product entity, int[] categoryIds);        
    }
}
