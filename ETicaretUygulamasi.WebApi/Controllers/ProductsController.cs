using Business.Abstract;
using Entities;
using ETicaretUygulamasi.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAll();
            var productsDTO = new List<ProductDTO>();
            foreach (var p in products)
            {
                productsDTO.Add(ProductToDTO(p));
            }

            return Ok(productsDTO);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product =  await _productService.GetById(id);
            if (product == null)
            {
                return NotFound(); // 404 error
            }
            return Ok(ProductToDTO(product)); // 200 status code
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, ProductToDTO(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,Product product)
        {
            if (id != product.ProductId)//requestte gonderdiği id ile doldurdugu yeni entity'nin id'leri eşleşmiyorsa
            {
                return BadRequest();
            }

            var entityFromDb = await _productService.GetById(id);

            if (entityFromDb == null)
            {
                return NotFound();
            }

            await _productService.UpdateAsync(entityFromDb, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(product);
            return NoContent();
        }

        private static ProductDTO ProductToDTO(Product p)
        {
            return new ProductDTO
            {
                Description = p.Description,
                ProductId = p.ProductId,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Price = p.Price,
                Url = p.Url
            };
        }
    }
}
