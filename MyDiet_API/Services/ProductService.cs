using MyDiet_API.Business.IRepository;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IList<ProductDto>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        public async Task<ProductDto> GetAsync(int id)
        {
            return await _productRepository.GetAsync(id);
        }

        public async Task<ProductDto> CreateAsync(ProductDto entityDto)
        {
            return await _productRepository.CreateAsync(entityDto);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductDto entityDto)
        {
            return await _productRepository.UpdateAsync(id, entityDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public bool CheckIfUnique(string parameter, ProductDto entityDto)
        {
            return _productRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
