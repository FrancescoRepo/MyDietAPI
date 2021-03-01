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
        [SwaggerOperation(Summary = "Get all Meals", Description = "Get all Meals")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /meals endpoint");
            IActionResult result;
            _logger.LogInformation("Retrieving all meals");
            var meals = await _mealService.GetAllAsync();
            _logger.LogDebug("Meals: {}", meals);

            result = Ok(meals);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get a Meal", Description = "Get a Meal with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /meals with id {}", id);
            IActionResult result;
            _logger.LogInformation("Retrieving meal");
            var meal = await _mealService.GetAsync(id);
            _logger.LogDebug("Meal: {}", meal);

            if (meal != null) result = Ok(meal);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a Meal", Description = "Create a new Meal")]
        [SwaggerResponse(201, "Created")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Create([FromBody] MealDto mealDto)
        {
            _logger.LogInformation("Entered in [POST] /meals endpoint with mealDto {}", mealDto);
            IActionResult result;
            var newMeal = await _mealService.CreateAsync(mealDto);
            _logger.LogDebug("New meal: {}", newMeal);

            if (newMeal != null) result = Created("", newMeal);
            else result = BadRequest();

            return result;
        }

        [HttpPost]
        [Route("{mealId}/diets/{dietId}")]
        [SwaggerOperation(Summary = "Add a Meal to a Diet", Description = "Add a Meal with a specific MealId to a Diet with a specific DietId")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
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
        [SwaggerOperation(Summary = "Remove a Meal from a Diet", Description = "Remove a Meal with a specific MealId from a Diet with a specific DietId")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> RemoveMealFromDiet(int mealId, int dietId)
        {
            _logger.LogInformation("Entered in [POST] RemoveMealFromDiet endpoint with mealId {}, dietId {}", mealId, dietId);
            IActionResult result;

            bool removed = await _mealService.RemoveMealFromDiet(dietId, mealId);
            if (removed) result = NoContent();
            else result = BadRequest();


            return result;
        }

        [HttpPost]
        [Route("{mealId}/products/{productId}")]
        [SwaggerOperation(Summary = "Add a Product to a Meal", Description = "Add a Product with a specific ProductId to a Meal with a specific MealId")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
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
        [SwaggerOperation(Summary = "Remove a Product from a Meal", Description = "Remove a Product with a specific ProductId from a Meal with a specific MealId")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> RemoveProductFromMeal(int mealId, int productId)
        {
            _logger.LogInformation("Entered in [POST] RemoveProductFromMeal endpoint with mealId {}, productId {}", mealId, productId);
            IActionResult result;

            bool removed = await _mealService.RemoveProductFromMeal(mealId, productId);
            if (removed) result = NoContent();
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a Meal", Description = "Update a Meal with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Update(int id, [FromBody] MealDto mealDto)
        {
            _logger.LogInformation("Entered in [PUT] /meals endpoint with id {}, mealDto {}", id, mealDto);
            IActionResult result;
            var updatedMeal = await _mealService.UpdateAsync(id, mealDto);
            _logger.LogDebug("Updated meal: {}", updatedMeal);

            if (updatedMeal != null) result = Ok(updatedMeal);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a Meal", Description = "Delete a Meal with a specific Id")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /meals endpoint with id {}", id);
            IActionResult result;

            if (await _mealService.DeleteAsync(id)) result = NoContent();
            else result = BadRequest();

            return result;
        }
    }
}
