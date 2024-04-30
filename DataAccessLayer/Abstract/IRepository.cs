using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
        Task<T> GetById(int Id);
        Task<List<T>> GetAll();
        void Create(T entity);
        Task CreateAsync(T entity);
        void Update(T entity);        
        void Delete(T entity);
    }
}
