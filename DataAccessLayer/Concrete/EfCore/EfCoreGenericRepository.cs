using DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext genericContext;
        public EfCoreGenericRepository(DbContext ctx)
        {
            genericContext = ctx;
        }
        public void Create(TEntity entity)
        {

            genericContext.Set<TEntity>().Add(entity);            

        }

        public async Task CreateAsync(TEntity entity)
        {

            await genericContext.Set<TEntity>().AddAsync(entity);

        }

        public void Delete(TEntity entity)
        {

            genericContext.Remove(entity);
            

        }

        public async Task<List<TEntity>> GetAll()
        {

            return await genericContext.Set<TEntity>().ToListAsync();

        }

        public async Task<TEntity> GetById(int Id)
        {
            return await genericContext.Set<TEntity>().FindAsync(Id);
        }

        public virtual void Update(TEntity entity) //virtual dedigimiz için bu generic classı inherit alan classın içinde Update'i override edebileceğiz
        {

            genericContext.Entry(entity).State = EntityState.Modified;
            

        }
        
    }
}
