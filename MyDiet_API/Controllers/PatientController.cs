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
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Entered in /patients endpoint");
            IActionResult result;
            try
            {
                _logger.LogInformation("Retrieving all patients");
                var patients = await _patientService.GetAll();
                _logger.LogDebug("Patient: {}", patients);
                result = Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in /patients endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Entered in /patients endpoint with id {}", id);
            IActionResult result;
            try
            {
                _logger.LogInformation("Retrieving patient");
                var patient = await _patientService.Get(id);
                _logger.LogDebug("Patient: {}", patient);

                if (patient != null) result = Ok(patient);
                else result = NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in /patients endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientDto patientDto)
        {
            _logger.LogInformation("Entered in [POST] /patients endpoint with patientDto {}", patientDto);
            IActionResult result;
            try
            {
                var newPatient = await _patientService.Create(patientDto);
                _logger.LogDebug("New Patient: {}", newPatient);

                if (newPatient != null) result = Created("", newPatient);
                else result = BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in [POST] /patients endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto patientDto)
        {
            _logger.LogError("Entered in [PUT] /patients endpoint with id {}, patientDto {}", id, patientDto);
            IActionResult result;
            try
            {
                var updatedPatient = await _patientService.Update(id, patientDto);
                _logger.LogDebug("Updated patient: {}", updatedPatient);

                if (updatedPatient != null) result = Ok(updatedPatient);
                else result = BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in [PUT] /patients endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Entered in [DELETE] /patients endpoint with id {}", id);
            IActionResult result;
            try
            {
                await _patientService.Delete(id);
                result = NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in [DELETE] /patients endpoint");
                result = StatusCode(500, new
                {
                    Message = "Internal Server Error"
                });
            }

            return result;
        }
    }
}
