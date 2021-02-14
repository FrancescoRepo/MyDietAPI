using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Business.IRepository
{
    public interface IMealRepository : IRepository<MealDto>
    {
        public Task<bool> AddMealToDiet(int dietId, int mealId);
        public Task<bool> AddProductToMeal(int mealId, int productId);
        public Task RemoveMealFromDiet(int dietId, int mealId);
        public Task RemoveProductFromMeal(int mealId, int productId);
    }
}
