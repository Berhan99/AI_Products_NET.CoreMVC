using Business.Abstract;
using Entities;
using ETicaretUygulamasi.WebUI.Models;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.Controllers
{
    //producutlarla alakalı
    public class ShopController:Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }

        //localhost/products/telefon?page=1
        public IActionResult List(string category,int page=1)
        {
            const int pageSize = 4;
            ProductListViewModels productViewModels = new ProductListViewModels()
            {
                PageInfo = new PageInfo() {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category

                },
                Products = _productService.GetProductsByCategory(category,page,pageSize)
            };

            return View(productViewModels);
        }

        public IActionResult Details(string url)
        {

            if (url == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductDetailModel { 
                Product = product,
                Categories = product.ProductCategories.Select(x=>x.Category).ToList()
            });           
        }


        public IActionResult Search(string searchFilter)
        {
            if (searchFilter == null)
            {
                return NotFound();
            }
            var productViewModel = new ProductListViewModels()
            {
                Products = _productService.GetSearchResult(searchFilter)
            };

            return View(productViewModel);            
        }

    }
}
