using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyDiet_API.Business.IRepository;
using MyDiet_API.Data;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Business
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;

        private const string NAME_PARAMETER = "Name";

        public ProductRepository(ApplicationDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<IList<ProductDto>> GetAll()
        {
            return _mapper.Map<IList<Product>, IList<ProductDto>>(await _ctx.Products.Include(p => p.ProductCategory).ToListAsync());
        }

        public async Task<ProductDto> Get(int id)
        {
            Product productFromDb = await _ctx.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
            ProductDto productDto = _mapper.Map<Product, ProductDto>(productFromDb);

            return productDto;
        }

        public async Task<ProductDto> Create(ProductDto entity)
        {
            Product productToAdd = _mapper.Map<ProductDto, Product>(entity);
            productToAdd.ProductCategory = null;
            Product newProduct = (await _ctx.Products.AddAsync(productToAdd)).Entity;

            await _ctx.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(newProduct);
        }

        public async Task<ProductDto> Update(int id, ProductDto entity)
        {
            Product productFromDb = await _ctx.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
            Product productToUpdate = _mapper.Map<ProductDto, Product>(entity);
            _ctx.Entry(productFromDb).CurrentValues.SetValues(productToUpdate);

            await _ctx.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(productToUpdate);
        }

        public async Task Delete(int id)
        {
            Product productFromDb = await _ctx.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
            _ctx.Products.Remove(productFromDb);

            await _ctx.SaveChangesAsync();
        }

        public bool CheckIfUnique(string parameter, ProductDto entity)
        {
            if(parameter.Equals(NAME_PARAMETER))
            {
                return _ctx.Products.Any(p => p.Name == entity.Name);
            }

            return false;
        }
    }
}
