using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Services.IService
{
    public interface IDietService : IService<DietDto>
    {
        public Task<DietDto> GetAllDietMeals(int id);
    }
}
