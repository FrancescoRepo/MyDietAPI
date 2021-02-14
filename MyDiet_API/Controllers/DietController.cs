using Microsoft.AspNetCore.Mvc;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System;
using System.Threading.Tasks;

namespace MyDiet_API.Controllers
{
    [ApiController]
    [Route("api/v1/diets")]
    public class DietController : Controller
    {
        private readonly IDietService _dietService;
        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IActionResult result;
            try
            {
                var diets = await _dietService.GetAll();
                result = Ok(diets);
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
                var diet = await _dietService.Get(id);
                if (diet != null) result = Ok(diet);
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

        [HttpGet]
        [Route("{id}/meals")]
        public async Task<IActionResult> GetAllDietMeals(int id)
        {
            IActionResult result;
            try
            {
                var dietDto = await _dietService.GetAllDietMeals(id);
                if (dietDto != null) result = Ok(dietDto);
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
        public async Task<IActionResult> Create([FromBody] DietDto dietDto)
        {
            IActionResult result;
            try
            {
                var newDiet = await _dietService.Create(dietDto);
                if (newDiet != null) result = Created("", newDiet);
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
        public async Task<IActionResult> Create(int id, [FromBody] DietDto dietDto)
        {
            IActionResult result;
            try
            {
                var updatedDiet = await _dietService.Update(id, dietDto);
                if (updatedDiet != null) result = Ok(updatedDiet);
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
                await _dietService.Delete(id);
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
