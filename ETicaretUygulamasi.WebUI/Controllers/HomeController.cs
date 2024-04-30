using Business.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;

using Entities;
using ETicaretUygulamasi.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult Index()
        {

            ProductListViewModels productViewModels = new ProductListViewModels()
            {
                Products = _productService.GetHomePageProducts()
            };

            return View(productViewModels);
        }
        public IActionResult About()
        {
            //return "homecontrollerı/aboutmethodu";
            return View();
        }

        public async Task<IActionResult> GetProductsFromRestApi()
        {
            var products = new List<Product>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44300/api/Products"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(products);
        }
    }
}
