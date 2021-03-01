using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Business.IRepository
{
    public interface IMealRepository : IRepository<MealDto>
    {
        public Task<bool> AddMealToDietAsync(int dietId, int mealId);
        public Task<bool> AddProductToMealAsync(int mealId, int productId);
        public Task<bool> RemoveMealFromDietAsync(int dietId, int mealId);
        public Task<bool> RemoveProductFromMealAsync(int mealId, int productId);
    }
}
