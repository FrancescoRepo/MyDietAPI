using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace MyDiet_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [SwaggerOperation(Summary = "Get Products", Description = "Get all Products")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /products endpoint");
            IActionResult result;
            _logger.LogInformation("Retrieving all products");
            var products = await _productService.GetAllAsync();
            result = Ok(products);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get a Product", Description = "Get a Product with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /products endpoint with id {}", id);
            IActionResult result;
            _logger.LogInformation("Retrieving product");
            var product = await _productService.GetAsync(id);
            _logger.LogDebug("Product: {}", product);

            if (product != null) result = Ok(product);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a Product", Description = "Create a new Product")]
        [SwaggerResponse(201, "Created")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            _logger.LogInformation("Entered in [POST] /products endpoint with productDto {}", productDto);
            IActionResult result;
            var newProduct = await _productService.CreateAsync(productDto);
            _logger.LogDebug("New Product: {}", newProduct);

            if (newProduct != null) result = Created("", newProduct);
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a Product", Description = "Update a Product with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            _logger.LogInformation("Entered in [PUT] /products endpoint with id {}, productDto {}", id, productDto);
            IActionResult result;
            var updatedProduct = await _productService.UpdateAsync(id, productDto);
            _logger.LogDebug("Updated Product: {}", updatedProduct);

            if (updatedProduct != null) result = Created("", updatedProduct);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a Product", Description = "Delete a Product with a specific Id")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /products endpoint with id {}", id);
            IActionResult result;

            if (await _productService.DeleteAsync(id)) result = NoContent();
            else result = BadRequest();

            return result;
        }
    }
}
