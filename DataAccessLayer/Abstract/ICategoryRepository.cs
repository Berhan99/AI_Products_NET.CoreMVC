using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {        
        Category GetByIdWithproducts(int categoryId);
        void DeleteFromCategory(int productId, int categoryId);
    }
}
