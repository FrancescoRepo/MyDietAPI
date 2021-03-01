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

        public async Task<IList<PatientDto>> GetAllAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<PatientDto> GetAsync(int id)
        {
            return await _patientRepository.GetAsync(id);
        }

        public async Task<PatientDto> CreateAsync(PatientDto entityDto)
        {
            return await _patientRepository.CreateAsync(entityDto);
        }

        public async Task<PatientDto> UpdateAsync(int id, PatientDto entityDto)
        {
            return await _patientRepository.UpdateAsync(id, entityDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _patientRepository.DeleteAsync(id);
        }

        public bool CheckIfUnique(string parameter, PatientDto entityDto)
        {
            return _patientRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
