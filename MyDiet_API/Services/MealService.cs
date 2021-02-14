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

        public async Task<IList<MealDto>> GetAll()
        {
            return await _mealRepository.GetAll();
        }

        public async Task<MealDto> Get(int id)
        {
            return await _mealRepository.Get(id);
        }
        public async Task<MealDto> Create(MealDto entityDto)
        {
            return await _mealRepository.Create(entityDto);
        }

        public async Task<MealDto> Update(int id, MealDto entityDto)
        {
            return await _mealRepository.Update(id, entityDto);
        }

        public async Task Delete(int id)
        {
            await _mealRepository.Delete(id);
        }

        public async Task<bool> AddMealToDiet(int dietId, int mealId)
        {
            return await _mealRepository.AddMealToDiet(dietId, mealId);
        }

        public async Task RemoveMealFromDiet(int dietId, int mealId)
        {
            await _mealRepository.RemoveMealFromDiet(dietId, mealId);
        }

        public bool CheckIfUnique(string parameter, MealDto entityDto)
        {
            return _mealRepository.CheckIfUnique(parameter, entityDto);
        }

        public async Task<bool> AddProductToMeal(int mealId, int productId)
        {
            return await _mealRepository.AddProductToMeal(mealId, productId);
        }

        public async Task RemoveProductFromMeal(int mealId, int productId)
        {
            await _mealRepository.RemoveProductFromMeal(mealId, productId);
        }
    }
}
