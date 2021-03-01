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
    [Route("api/v1/patients")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Patients", Description = "Get all Patients")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /patients endpoint");
            IActionResult result;

            _logger.LogInformation("Retrieving all patients");
            var patients = await _patientService.GetAllAsync();
            _logger.LogDebug("Patients: {}", patients);

            result = Ok(patients);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get a Patient", Description = "Get a Patient with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(404, "Not Found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /patients endpoint with id {}", id);
            IActionResult result;

            _logger.LogInformation("Retrieving patient");
            var patient = await _patientService.GetAsync(id);
            _logger.LogDebug("Patient: {}", patient);

            if (patient != null) result = Ok(patient);
            else result = NotFound();

            return result;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a Patient", Description = "Create a new Patient")]
        [SwaggerResponse(201, "Created")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Create([FromBody] PatientDto patientDto)
        {
            _logger.LogInformation("Entered in [POST] /patients endpoint with patientDto {}", patientDto);
            IActionResult result;

            var newPatient = await _patientService.CreateAsync(patientDto);
            _logger.LogDebug("New Patient: {}", newPatient);

            if (newPatient != null) result = Created("", newPatient);
            else result = BadRequest();

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a Patient", Description = "Update a Patient with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto patientDto)
        {
            _logger.LogError("Entered in [PUT] /patients endpoint with id {}, patientDto {}", id, patientDto);
            IActionResult result;
            var updatedPatient = await _patientService.UpdateAsync(id, patientDto);
            _logger.LogDebug("Updated patient: {}", updatedPatient);

            if (updatedPatient != null) result = Ok(updatedPatient);
            else result = BadRequest();

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a Patient", Description = "Delete a Patient with a specific Id")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /patients endpoint with id {}", id);
            IActionResult result;

            if (await _patientService.DeleteAsync(id)) result = NoContent();
            else result = BadRequest();

            return result;
        }
    }
}
