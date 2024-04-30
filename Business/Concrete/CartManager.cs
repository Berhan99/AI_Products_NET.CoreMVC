using Business.Abstract;
using DataAccessLayer.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;        

        public CartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddToCart(string UserId, int productId, int quantity)
        {
            var cart = GetCartByUserId(UserId);

            if (cart != null)
            {
                //eklenmek istenen ürün sepette varmı(güncelleme)
                //eklenmek isteyen ürün sepette var ve yeni kayıt oluştur

                var index = cart.CartItems.FindIndex(i => i.ProductId == productId);

                if (index < 0)//kayıt yok yeni kayıt olustur
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = quantity
                    });
                }
                else // kayıt var yeni kayıt ekleme quantity güncelle
                {
                    cart.CartItems[index].Quantity += quantity;
                }

                _unitOfWork.Carts.Update(cart);
                _unitOfWork.Save();
            }
        }

        public void ClearCart(int cartId)
        {
            _unitOfWork.Carts.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);

            if (cart != null)
            {
                _unitOfWork.Carts.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart GetCartByUserId(string UserId)
        {
            return _unitOfWork.Carts.GetByUserId(UserId);
        }

        public void InitializeCart(string UserId)
        {
            _unitOfWork.Carts.Create(new Cart() { UserId = UserId });
            _unitOfWork.Save();
        }
    }
}
