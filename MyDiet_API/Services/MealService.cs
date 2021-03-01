using MyDiet_API.Business.IRepository;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<IList<MealDto>> GetAllAsync()
        {
            return await _mealRepository.GetAllAsync();
        }

        public async Task<MealDto> GetAsync(int id)
        {
            return await _mealRepository.GetAsync(id);
        }
        public async Task<MealDto> CreateAsync(MealDto entityDto)
        {
            return await _mealRepository.CreateAsync(entityDto);
        }

        public async Task<MealDto> UpdateAsync(int id, MealDto entityDto)
        {
            return await _mealRepository.UpdateAsync(id, entityDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _mealRepository.DeleteAsync(id);
        }

        public async Task<bool> AddMealToDiet(int dietId, int mealId)
        {
            return await _mealRepository.AddMealToDietAsync(dietId, mealId);
        }

        public async Task<bool> RemoveMealFromDiet(int dietId, int mealId)
        {
            return await _mealRepository.RemoveMealFromDietAsync(dietId, mealId);
        }

        public bool CheckIfUnique(string parameter, MealDto entityDto)
        {
            return _mealRepository.CheckIfUnique(parameter, entityDto);
        }

        public async Task<bool> AddProductToMeal(int mealId, int productId)
        {
            return await _mealRepository.AddProductToMealAsync(mealId, productId);
        }

        public async Task<bool> RemoveProductFromMeal(int mealId, int productId)
        {
            return await _mealRepository.RemoveProductFromMealAsync(mealId, productId);
        }
    }
}
