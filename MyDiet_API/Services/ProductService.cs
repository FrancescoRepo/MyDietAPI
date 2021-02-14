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

        public async Task<IList<ProductDto>> GetAll()
        {
            return await _productRepository.GetAll();
        }
        public async Task<ProductDto> Get(int id)
        {
            return await _productRepository.Get(id);
        }

        public async Task<ProductDto> Create(ProductDto entityDto)
        {
            return await _productRepository.Create(entityDto);
        }

        public async Task<ProductDto> Update(int id, ProductDto entityDto)
        {
            return await _productRepository.Update(id, entityDto);
        }

        public async Task Delete(int id)
        {
            await _productRepository.Delete(id);
        }

        public bool CheckIfUnique(string parameter, ProductDto entityDto)
        {
            return _productRepository.CheckIfUnique(parameter, entityDto);
        }
    }
}
