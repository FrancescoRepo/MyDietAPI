using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Business.IRepository
{
    public interface IDietRepository : IRepository<DietDto>
    {
        public Task<DietDto> GetAllDietMealsAsync(int id);
    }
}
