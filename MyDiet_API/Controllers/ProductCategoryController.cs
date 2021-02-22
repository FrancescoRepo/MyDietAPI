using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
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
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /productcategories endpoint");
            IActionResult result;
            _logger.LogInformation("Retrieving all productCategories");
            var productCategories = await _productCategoryService.GetAll();
            result = Ok(productCategories);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /productcategories endpoint with id {}", id);
            IActionResult result;
            _logger.LogInformation("Retrieving product category");
            var productCategory = await _productCategoryService.Get(id);
            _logger.LogDebug("productCategory: {}", productCategory);

            if (productCategory != null) result = Ok(productCategory);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Entered in [POST] /productcategories endpoint with {}", productCategoryDto);
            IActionResult result;
            var newProductCategory = await _productCategoryService.Create(productCategoryDto);
            _logger.LogDebug("New ProductCategory: {}", newProductCategory);

            if (newProductCategory != null) result = Created("", newProductCategory);
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Entered in [PUT] /productcategories endpoint with id {}, productCategoryDto {}", id, productCategoryDto);
            IActionResult result;
            var updatedProductCategory = await _productCategoryService.Update(id, productCategoryDto);
            _logger.LogDebug("Updated ProductCategory: {}", updatedProductCategory);
            if (updatedProductCategory != null) result = Ok(updatedProductCategory);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /productcategories endpoint with id {}", id);
            IActionResult result;
            await _productCategoryService.Delete(id);
            result = NoContent();

            return result;
        }
    }
}
