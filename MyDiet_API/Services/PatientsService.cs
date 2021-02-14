using MyDiet_API.Business.IRepository;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services
{
    public class PatientsService : IPatientService
    {
        private IPatientRepository _patientRepository;

        public PatientsService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IList<PatientDto>> GetAll()
        {
            return await _patientRepository.GetAll();
        }

        public async Task<PatientDto> Get(int id)
        {
            return await _patientRepository.Get(id);
        }

        public async Task<PatientDto> Create(PatientDto entityDto)
        {
            return await _patientRepository.Create(entityDto);
        }

        public async Task<PatientDto> Update(int id, PatientDto entityDto)
        {
            return await _patientRepository.Update(id, entityDto);
        }

        public async Task Delete(int id)
        {
            await _patientRepository.Delete(id);
        }

        public bool CheckIfUnique(string parameter, PatientDto entityDto)
        {
            return _patientRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
