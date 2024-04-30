using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface ICartRepository:IRepository<Cart>
    {
        Cart GetByUserId(string UserId);
        void DeleteFromCart(int CartId, int productId);
        void ClearCart(int cartId);
    }
}
