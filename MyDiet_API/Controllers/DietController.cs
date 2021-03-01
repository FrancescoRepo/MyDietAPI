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
    [Route("api/v1/diets")]
    public class DietController : Controller
    {
        private readonly IDietService _dietService;
        private readonly ILogger<DietController> _logger;

        public DietController(IDietService dietService, ILogger<DietController> logger)
        {
            _dietService = dietService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Diets", Description = "Get all Diets")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /diets endpoint");
            IActionResult result;
            _logger.LogInformation("Retrieving all diets");
            var diets = await _dietService.GetAllAsync();
            _logger.LogDebug("Diets: {}", diets);

            result = Ok(diets);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get a Diet", Description = "Get a Diet with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /meals endpoint with id {}", id);
            IActionResult result;

            _logger.LogInformation("Retrieving diet");
            var diet = await _dietService.GetAsync(id);
            _logger.LogDebug("Diet: {}", diet);

            if (diet != null) result = Ok(diet);
            else result = NotFound();

            return result;
        }

        [HttpGet]
        [Route("{id}/meals")]
        [SwaggerOperation(Summary = "Get all Diet Meals", Description = "Get all Diet Meals with a specific DietId")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> GetAllDietMeals(int id)
        {
            _logger.LogInformation("Entered in GetAllDietMeals endpoint with id {}", id);
            IActionResult result;

            _logger.LogInformation("Retrieving diet {} with all associated meals", id);
            var dietDto = await _dietService.GetAllDietMeals(id);
            _logger.LogDebug("Diet: {}", dietDto);

            if (dietDto != null) result = Ok(dietDto);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a Diet", Description = "Create a new Diet")]
        [SwaggerResponse(201, "Created")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Create([FromBody] DietDto dietDto)
        {
            _logger.LogInformation("Entered in [POST] /diets endpoint with dietDto {}", dietDto);
            IActionResult result;

            var newDiet = await _dietService.CreateAsync(dietDto);
            if (newDiet != null) result = Created("", newDiet);
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a Diet", Description = "Update a Diet with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Update(int id, [FromBody] DietDto dietDto)
        {
            _logger.LogInformation("Entered in [PUT] /diets endpoint with id {}, dietDto {}", id, dietDto);
            IActionResult result;
            var updatedDiet = await _dietService.UpdateAsync(id, dietDto);
            _logger.LogDebug("Updated diet: {}", updatedDiet);

            if (updatedDiet != null) result = Ok(updatedDiet);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a Diet", Description = "Delete a Diet with a specific Id")]
        [SwaggerResponse(204, "No Content")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /diets endpoint with id {}", id);
            IActionResult result;

            if (await _dietService.DeleteAsync(id)) result = NoContent();
            else result = BadRequest();

            return result;
        }
    }
}
