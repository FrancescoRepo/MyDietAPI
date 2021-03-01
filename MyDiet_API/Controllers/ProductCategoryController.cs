using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/v1/productcategories")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly ILogger<ProductCategoryController> _logger;
        public ProductCategoryController(IProductCategoryService productCategoryService, ILogger<ProductCategoryController> logger)
        {
            _productCategoryService = productCategoryService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get Product Categories", Description = "Get all Product Categories")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /productcategories endpoint");

            IActionResult result;
            _logger.LogInformation("Retrieving all productCategories");

            result = Ok(await _productCategoryService.GetAllAsync());

            return result;
        }
        
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get Product Category", Description = "Get a Product Category with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /productcategories endpoint with id {}", id);
            IActionResult result;

            _logger.LogInformation("Retrieving product category");

            var productCategory = await _productCategoryService.GetAsync(id);
            _logger.LogDebug("productCategory: {}", productCategory);

            if (productCategory != null) result = Ok(productCategory);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a Product Category", Description = "Create a new Product Category")]
        [SwaggerResponse(201, "Created")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Entered in [POST] /productcategories endpoint with {}", productCategoryDto);
            IActionResult result;

            var newProductCategory = await _productCategoryService.CreateAsync(productCategoryDto);
            _logger.LogDebug("New ProductCategory: {}", newProductCategory);

            if (newProductCategory != null) result = Created("", newProductCategory);
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a Product Category", Description = "Update a Product Category with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Entered in [PUT] /productcategories endpoint with id {}, productCategoryDto {}", id, productCategoryDto);
            IActionResult result;

            var updatedProductCategory = await _productCategoryService.UpdateAsync(id, productCategoryDto);
            _logger.LogDebug("Updated ProductCategory: {}", updatedProductCategory);

            if (updatedProductCategory != null) result = Ok(updatedProductCategory);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a Product Category", Description = "Delete a Product Category with a specific Id")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /productcategories endpoint with id {}", id);
            IActionResult result;

            if (await _productCategoryService.DeleteAsync(id)) result = NoContent();
            else result = BadRequest();

            return result;
        }
    }
}
