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
    [Route("api/v1/patients")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IActionResult result;
            try
            {
                var patients = await _patientService.GetAll();
                result = Ok(patients);
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
                var patient = await _patientService.Get(id);
                if (patient != null) result = Ok(patient);
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
        public async Task<IActionResult> Create([FromBody] PatientDto patientDto)
        {
            IActionResult result;
            try
            {
                var newPatient = await _patientService.Create(patientDto);
                if (newPatient != null) result = Created("", newPatient);
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
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto patientDto)
        {
            IActionResult result;
            try
            {
                var updatedPatient = await _patientService.Update(id, patientDto);
                if (updatedPatient != null) result = Ok(updatedPatient);
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
                await _patientService.Delete(id);
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
