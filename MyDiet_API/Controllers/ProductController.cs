using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /products endpoint");
            IActionResult result;
            try
            {
                _logger.LogInformation("Retrieving all products");
                var products = await _productService.GetAll();
                result = Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in /products endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /products endpoint with id {}", id);
            IActionResult result;
            try
            {
                _logger.LogInformation("Retrieving product");
                var product = await _productService.Get(id);
                _logger.LogDebug("Product: {}", product);

                if (product != null) result = Ok(product);
                else result = NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in /products endpoint with id {}", id);
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            _logger.LogInformation("Entered in [POST] /products endpoint with productDto {}", productDto);
            IActionResult result;
            try
            {
                var newProduct = await _productService.Create(productDto);
                _logger.LogDebug("New Product: {}", newProduct);

                if (newProduct != null) result = Created("", newProduct);
                else result = BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in [POST] /products endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            _logger.LogInformation("Entered in [PUT] /products endpoint with id {}, productDto {}", id, productDto);
            IActionResult result;
            try
            {
                var updatedProduct = await _productService.Update(id, productDto);
                _logger.LogDebug("Updated Product: {}", updatedProduct);

                if (updatedProduct != null) result = Created("", updatedProduct);
                else result = BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in [PUT] /products endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /products endpoint with id {}", id);
            IActionResult result;
            try
            {
                await _productService.Delete(id);
                result = NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in [DELETE] /products endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }
    }
}
