using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyDiet_API.Business.IRepository;
using MyDiet_API.Data;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Shared.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Business
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRepository> _logger;

        private const string NAME_PARAMETER = "Name";

        public ProductRepository(ApplicationDbContext ctx, IMapper mapper, ILogger<ProductRepository> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<ProductDto>> GetAllAsync()
        {
            _logger.LogInformation("Entered in GetAllAsync");
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            List<Product> products = await _ctx.Products.Include(p => p.ProductCategory).ToListAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<IList<Product>, IList<ProductDto>>(products);
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            _logger.LogInformation("Entered in Get with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Product productFromDb = await _ctx.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);

            if (productFromDb == null)
            {
                _logger.LogError("Product with id {} not found", id);
                return null;
            }

            ProductDto productDto = _mapper.Map<Product, ProductDto>(productFromDb);

            return productDto;
        }

        public async Task<ProductDto> CreateAsync(ProductDto entity)
        {
            _logger.LogInformation("Entered in Create with productDto {}", entity);
            Stopwatch stopWatch = new Stopwatch();

            Product productToAdd = _mapper.Map<ProductDto, Product>(entity);
            productToAdd.ProductCategory = null;

            stopWatch.Start();

            Product newProduct = (await _ctx.Products.AddAsync(productToAdd)).Entity;

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);

            return _mapper.Map<Product, ProductDto>(newProduct);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductDto entity)
        {
            _logger.LogInformation("Entered in Update with id {}, productDto {}", id, entity);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            
            Product productFromDb = await _ctx.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
            
            if(productFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Product with id {} not found", id);

                return null;
            }

            Product productToUpdate = _mapper.Map<ProductDto, Product>(entity);
            _ctx.Entry(productFromDb).CurrentValues.SetValues(productToUpdate);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);

            return _mapper.Map<Product, ProductDto>(productToUpdate);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Entered in Delete with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            Product productFromDb = await _ctx.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
            
            if(productFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Product with id {} not found", id);

                return false;
            }
            _ctx.Products.Remove(productFromDb);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);

            return true;
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
