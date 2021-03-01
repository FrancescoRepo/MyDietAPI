using MyDiet_API.Business.IRepository;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services
{
    public class DietService : IDietService
    {
        private readonly IDietRepository _dietRepository;
        public DietService(IDietRepository dietRepository)
        {
            _dietRepository = dietRepository;
        }

        public async Task<IList<DietDto>> GetAllAsync()
        {
            return await _dietRepository.GetAllAsync();
        }

        public async Task<DietDto> GetAsync(int id)
        {
            return await _dietRepository.GetAsync(id);
        }

        public async Task<DietDto> CreateAsync(DietDto entityDto)
        {
            return await _dietRepository.CreateAsync(entityDto);
        }

        public async Task<DietDto> UpdateAsync(int id, DietDto entityDto)
        {
            return await _dietRepository.UpdateAsync(id, entityDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _dietRepository.DeleteAsync(id);
        }

        public async Task<DietDto> GetAllDietMeals(int id)
        {
            return await _dietRepository.GetAllDietMealsAsync(id);
        }

        public bool CheckIfUnique(string parameter, DietDto entityDto)
        {
            return _dietRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
