using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string UserId);
        Cart GetCartByUserId(string UserId);

        void AddToCart(string UserId,int productId,int quantity);
        void DeleteFromCart(string userId, int productId);
        void ClearCart(int cartId);
    }
}
