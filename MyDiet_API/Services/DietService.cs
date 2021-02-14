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

        public async Task<IList<DietDto>> GetAll()
        {
            return await _dietRepository.GetAll();
        }

        public async Task<DietDto> Get(int id)
        {
            return await _dietRepository.Get(id);
        }

        public async Task<DietDto> Create(DietDto entityDto)
        {
            return await _dietRepository.Create(entityDto);
        }

        public async Task<DietDto> Update(int id, DietDto entityDto)
        {
            return await _dietRepository.Update(id, entityDto);
        }

        public async Task Delete(int id)
        {
            await _dietRepository.Delete(id);
        }

        public async Task<DietDto> GetAllDietMeals(int id)
        {
            return await _dietRepository.GetAllDietMeals(id);
        }

        public bool CheckIfUnique(string parameter, DietDto entityDto)
        {
            return _dietRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
