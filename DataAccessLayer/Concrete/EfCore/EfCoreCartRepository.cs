using DataAccessLayer.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart>, ICartRepository
    {
        public EfCoreCartRepository(ETicaretUygulamasiContext ctx) : base(ctx)
        {

        }
        private ETicaretUygulamasiContext Context
        {
            get
            {
                return genericContext as ETicaretUygulamasiContext;
            }
        }
        public void ClearCart(int cartId)
        {

            var cmd = @"Delete from CartItems where CartId=@p0 ";
            Context.Database.ExecuteSqlRaw(cmd, cartId);

        }

        public void DeleteFromCart(int CartId, int productId)
        {

            var cmd = @"Delete from CartItems where CartId=@p0 and ProductId=@p1";
            Context.Database.ExecuteSqlRaw(cmd, CartId, productId);

        }

        public Cart GetByUserId(string UserId)
        {

            return Context.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.UserId == UserId);

        }

        public override void Update(Cart entity)
        {

            //context.Entry(entity).State = EntityState.Modified; yerine context.Update(entity);
            //yapıldığında entitynin içerisindeki kolonlar ilişkili olduğu diğer tablolardaki alanları da update olur
            Context.Update(entity);
            

        }
    }
}
