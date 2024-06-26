﻿using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService : IValidator<Category>
    {
        Task<Category> GetById(int Id);
        Category GetByIdWithProducts(int categoryId);
        Task<List<Category>> GetAll();
        void Create(Category entity);
        Task<Category> CreateAsync(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        void DeleteFromCategory(int productId, int categoryId);


    }
}
