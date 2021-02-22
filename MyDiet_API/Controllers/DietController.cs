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
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /diets endpoint");
            IActionResult result;
            _logger.LogInformation("Retrieving all diets");
            var diets = await _dietService.GetAll();
            _logger.LogDebug("Diets: {}", diets);

            result = Ok(diets);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /meals endpoint with id {}", id);
            IActionResult result;
            _logger.LogInformation("Retrieving diet");
            var diet = await _dietService.Get(id);
            _logger.LogDebug("Diet: {}", diet);

            if (diet != null) result = Ok(diet);
            else result = NotFound();

            return result;
        }

        [HttpGet]
        [Route("{id}/meals")]
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
        public async Task<IActionResult> Create([FromBody] DietDto dietDto)
        {
            _logger.LogInformation("Entered in [POST] /diets endpoint with dietDto {}", dietDto);
            IActionResult result;
            var newDiet = await _dietService.Create(dietDto);
            if (newDiet != null) result = Created("", newDiet);
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DietDto dietDto)
        {
            _logger.LogInformation("Entered in [PUT] /diets endpoint with id {}, dietDto {}", id, dietDto);
            IActionResult result;
            var updatedDiet = await _dietService.Update(id, dietDto);
            _logger.LogDebug("Updated diet: {}", updatedDiet);

            if (updatedDiet != null) result = Ok(updatedDiet);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /diets endpoint with id {}", id);
            IActionResult result;
            await _dietService.Delete(id);
            result = NoContent();

            return result;
        }
    }
}
