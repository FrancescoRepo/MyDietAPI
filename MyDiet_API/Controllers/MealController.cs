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
    [Route("api/v1/meals")]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IActionResult result;
            try
            {
                var meals = await _mealService.GetAll();
                result = Ok(meals);
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
                var meal = await _mealService.Get(id);
                if (meal != null) result = Ok(meal);
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
        public async Task<IActionResult> Create([FromBody] MealDto mealDto)
        {
            IActionResult result;
            try
            {
                var newMeal = await _mealService.Create(mealDto);
                if (newMeal != null) result = Created("", newMeal);
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

        [HttpPost]
        [Route("{mealId}/diets/{dietId}")]
        public async Task<IActionResult> AddMealtoDiet(int mealId, int dietId)
        {
            IActionResult result;
            try
            {
                bool added = await _mealService.AddMealToDiet(dietId, mealId);
                if (added) result = Ok(); else result = BadRequest();
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
        [Route("{mealId}/diets/{dietId}")]
        public async Task<IActionResult> RemoveMealFromDiet(int mealId, int dietId)
        {
            IActionResult result;
            try
            {
                await _mealService.RemoveMealFromDiet(dietId, mealId);
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

        [HttpPost]
        [Route("{mealId}/products/{productId}")]
        public async Task<IActionResult> AddProductToMeal(int mealId, int productId)
        {
            IActionResult result;
            try
            {
                bool added = await _mealService.AddProductToMeal(mealId, productId);
                if (added) result = Ok(); else result = BadRequest();
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
        [Route("{mealId}/products/{productId}")]
        public async Task<IActionResult> RemoveProductFromMeal(int mealId, int productId)
        {
            IActionResult result;
            try
            {
                await _mealService.RemoveProductFromMeal(mealId, productId);
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

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MealDto mealDto)
        {
            IActionResult result;
            try
            {
                var updatedMeal = await _mealService.Update(id, mealDto);
                if (updatedMeal != null) result = Ok(updatedMeal);
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
                await _mealService.Delete(id);
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
