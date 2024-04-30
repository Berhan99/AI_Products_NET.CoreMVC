using Business.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;        

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public bool Create(Product entity)
        {
            //iş kuralları kontroller vs

            if (Validation(entity))
            {
                _unitOfWork.Products.Create(entity);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            await _unitOfWork.Products.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
            return entity;
        }

        public void Delete(Product entity)
        {
            //iş kuralları kontroller vs
            _unitOfWork.Products.Delete(entity);
            _unitOfWork.Save();
        }

        public async Task<List<Product>> GetAll()
        {
            return await _unitOfWork.Products.GetAll();
        }

        public async Task<Product> GetById(int Id)
        {
            return await _unitOfWork.Products.GetById(Id);
        }

        public Product GetByIdWithCategories(int Id)
        {
            return _unitOfWork.Products.GetByIdWithCategories(Id);
        }

        public int GetCountByCategory(string category)
        {
            return _unitOfWork.Products.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _unitOfWork.Products.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _unitOfWork.Products.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _unitOfWork.Products.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchFilter)
        {
            return _unitOfWork.Products.GetSearchResult(searchFilter);        
        }

        public void Update(Product entity)
        {
            _unitOfWork.Products.Update(entity);
            _unitOfWork.Save();
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "ürün için en az bir kategori seçmelisiniz";
                    return false;
                }
                _unitOfWork.Products.Update(entity, categoryIds);
                _unitOfWork.Save();
                return true;
            }

            return false;
            
        }

        public string ErrorMessage { get ; set; }
        public bool Validation(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz.\n";
                isValid = false;
            }

            if (entity.Price < 0)
            {
                ErrorMessage += "ürün fiyatı negatif olamaz.\n";
                isValid = false;
            }


            return isValid;
        }

        public async Task UpdateAsync(Product entityFromDb, Product product)
        {
            entityFromDb.Name = product.Name;
            entityFromDb.Price= product.Price;
            entityFromDb.Description = product.Description;
            
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Product entity)
        {
            _unitOfWork.Products.Delete(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}
