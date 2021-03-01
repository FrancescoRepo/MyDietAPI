using MyDiet_API.Business.IRepository;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services
{
    public class ProductCategoriesService : IProductCategoryService
    {
        private IProductCategoryRepository _productCategoryRepository;

        public ProductCategoriesService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<IList<ProductCategoryDto>> GetAllAsync()
        {
            return await _productCategoryRepository.GetAllAsync();
        }
        public async Task<ProductCategoryDto> GetAsync(int id)
        {
            return await _productCategoryRepository.GetAsync(id);
        }

        public async Task<ProductCategoryDto> CreateAsync(ProductCategoryDto productCategoryDto)
        {
            return await _productCategoryRepository.CreateAsync(productCategoryDto);
        }

        public async Task<ProductCategoryDto> UpdateAsync(int id, ProductCategoryDto productCategoryDto)
        {
            return await _productCategoryRepository.UpdateAsync(id, productCategoryDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productCategoryRepository.DeleteAsync(id);
        }

        public bool CheckIfUnique(string parameter, ProductCategoryDto entityDto)
        {
            return _productCategoryRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
