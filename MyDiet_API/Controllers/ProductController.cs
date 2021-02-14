using Microsoft.AspNetCore.Mvc;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System;
using System.Threading.Tasks;

namespace MyDiet_API.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IActionResult result;
            try
            {
                var products = await _productService.GetAll();
                result = Ok(products);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new
                {
                    Message = "Something went wrong",
                    Exception = ex.Message
                });
            }

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            IActionResult result;
            try
            {
                var product = await _productService.Get(id);
                if (product != null) result = Ok(product);
                else result = NotFound();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new
                {
                    Message = "Something went wrong",
                    Exception = ex.Message
                });
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            IActionResult result;
            try
            {
                var newProduct = await _productService.Create(productDto);
                if (newProduct != null) result = Created("", newProduct);
                else result = BadRequest();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new
                {
                    Message = "Something went wrong",
                    Exception = ex.Message
                });
            }

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            IActionResult result;
            try
            {
                var updatedProduct = await _productService.Update(id, productDto);
                if (updatedProduct != null) result = Created("", updatedProduct);
                else result = BadRequest();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new
                {
                    Message = "Something went wrong",
                    Exception = ex.Message
                });
            }

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result;
            try
            {
                await _productService.Delete(id);
                result = NoContent();
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new
                {
                    Message = "Something went wrong",
                    Exception = ex.Message
                });
            }

            return result;
        }
    }
}
