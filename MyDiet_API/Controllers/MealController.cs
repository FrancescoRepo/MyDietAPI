using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MealController> _logger;

        public MealController(IMealService mealService, ILogger<MealController> logger)
        {
            _mealService = mealService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /meals endpoint");
            IActionResult result;
            _logger.LogInformation("Retrieving all meals");
            var meals = await _mealService.GetAll();
            _logger.LogDebug("Meals: {}", meals);

            result = Ok(meals);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /meals with id {}", id);
            IActionResult result;
            _logger.LogInformation("Retrieving meal");
            var meal = await _mealService.Get(id);
            _logger.LogDebug("Meal: {}", meal);

            if (meal != null) result = Ok(meal);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MealDto mealDto)
        {
            _logger.LogInformation("Entered in [POST] /meals endpoint with mealDto {}", mealDto);
            IActionResult result;
            var newMeal = await _mealService.Create(mealDto);
            _logger.LogDebug("New meal: {}", newMeal);

            if (newMeal != null) result = Created("", newMeal);
            else result = BadRequest();

            return result;
        }

        [HttpPost]
        [Route("{mealId}/diets/{dietId}")]
        public async Task<IActionResult> AddMealtoDiet(int mealId, int dietId)
        {
            _logger.LogInformation("Entered in [POST] AddMealToDiet endpoint with mealId {}, dietId {}", mealId, dietId);
            IActionResult result;
            bool added = await _mealService.AddMealToDiet(dietId, mealId);
            if (added) result = Ok(); else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{mealId}/diets/{dietId}")]
        public async Task<IActionResult> RemoveMealFromDiet(int mealId, int dietId)
        {
            _logger.LogInformation("Entered in [POST] RemoveMealFromDiet endpoint with mealId {}, dietId {}", mealId, dietId);
            IActionResult result;
            await _mealService.RemoveMealFromDiet(dietId, mealId);
            result = NoContent();

            return result;
        }

        [HttpPost]
        [Route("{mealId}/products/{productId}")]
        public async Task<IActionResult> AddProductToMeal(int mealId, int productId)
        {
            _logger.LogInformation("Entered in [POST] AddProductToMeal endpoint with mealId {}, productId {}", mealId, productId);
            IActionResult result;
            bool added = await _mealService.AddProductToMeal(mealId, productId);
            if (added) result = Ok(); else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{mealId}/products/{productId}")]
        public async Task<IActionResult> RemoveProductFromMeal(int mealId, int productId)
        {
            _logger.LogInformation("Entered in [POST] RemoveProductFromMeal endpoint with mealId {}, productId {}", mealId, productId);
            IActionResult result;
            await _mealService.RemoveProductFromMeal(mealId, productId);
            result = NoContent();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MealDto mealDto)
        {
            _logger.LogInformation("Entered in [PUT] /meals endpoint with id {}, mealDto {}", id, mealDto);
            IActionResult result;
            var updatedMeal = await _mealService.Update(id, mealDto);
            _logger.LogDebug("Updated meal: {}", updatedMeal);

            if (updatedMeal != null) result = Ok(updatedMeal);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /meals endpoint with id {}", id);
            IActionResult result;
            await _mealService.Delete(id);
            result = NoContent();

            return result;
        }
    }
}
