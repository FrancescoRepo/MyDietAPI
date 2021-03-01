using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Services.IService
{
    public interface IMealService : IService<MealDto>
    {
        public Task<bool> AddMealToDiet(int dietId, int mealId);
        public Task<bool> AddProductToMeal(int mealId, int productId);
        public Task<bool> RemoveMealFromDiet(int dietId, int mealId);
        public Task<bool> RemoveProductFromMeal(int mealId, int productId);
    }
}
