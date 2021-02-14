using Microsoft.AspNetCore.Mvc;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Controllers
{
    [ApiController]
    [Route("api/v1/productcategories")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IActionResult result;
            try
            {
                var productCategories = await _productCategoryService.GetAll();
                result = Ok(productCategories);
            }
            catch (Exception)
            {
                result = StatusCode(500, new
                {
                    Message = "Something went wrong"
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
                var productCategory = await _productCategoryService.Get(id);
                if (productCategory != null) result = Ok(productCategory);
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
        public async Task<IActionResult> Create([FromBody] ProductCategoryDto productCategoryDto)
        {
            IActionResult result;
            try
            {
                var newProductCategory = await _productCategoryService.Create(productCategoryDto);
                if (newProductCategory != null) result = Created("", newProductCategory);
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
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryDto productCategoryDto)
        {
            IActionResult result;
            try
            {
                var updatedProductCategory = await _productCategoryService.Update(id, productCategoryDto);
                if (updatedProductCategory != null) result = Ok(updatedProductCategory);
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
                await _productCategoryService.Delete(id);
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
