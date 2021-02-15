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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCategoryRepository> _logger;

        private const string DESCRIPTION_PARAMETER = "Description";

        public ProductCategoryRepository(ApplicationDbContext ctx, IMapper mapper, ILogger<ProductCategoryRepository> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<ProductCategoryDto>> GetAll()
        {
            _logger.LogInformation("Entered in GetAll");
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Start();
            
            List<ProductCategory> productCategories = await _ctx.ProductCategories.ToListAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<IList<ProductCategory>, IList<ProductCategoryDto>>(productCategories);
        }

        public async Task<ProductCategoryDto> Get(int id)
        {
            _logger.LogInformation("Entered in Get with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            ProductCategory productCategory = await _ctx.ProductCategories.FindAsync(id);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
        }

        public async Task<ProductCategoryDto> Create(ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Entered in Create with productCategoryDto {}", productCategoryDto);
            Stopwatch stopWatch = new Stopwatch();

            ProductCategory productCategory = _mapper.Map<ProductCategoryDto, ProductCategory>(productCategoryDto);

            stopWatch.Start();

            ProductCategory newProductCategory = (await _ctx.ProductCategories.AddAsync(productCategory)).Entity;
            
            await _ctx.SaveChangesAsync();
            
            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<ProductCategory, ProductCategoryDto>(newProductCategory);
        }

        public async Task<ProductCategoryDto> Update(int id, ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Entered in Update with id {}, productCategoryDto {}", id, productCategoryDto);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            ProductCategory productCategoryFromDb = await _ctx.ProductCategories.FindAsync(id);
            ProductCategory productCategoryToUpdate = _mapper.Map<ProductCategoryDto, ProductCategory>(productCategoryDto);
            _ctx.Entry(productCategoryFromDb).CurrentValues.SetValues(productCategoryToUpdate);
            
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<ProductCategory, ProductCategoryDto>(productCategoryToUpdate);
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Entered in Delete with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            
            ProductCategory productCategoryFromDb = await _ctx.ProductCategories.FindAsync(id);
            _ctx.ProductCategories.Remove(productCategoryFromDb);
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time is {0} ms", stopWatch.ElapsedMilliseconds);
        }

        public bool CheckIfUnique(string parameter, ProductCategoryDto entity)
        {
            if(parameter.Equals(DESCRIPTION_PARAMETER))
            {
                if(entity.Id == 0)
                {
                    return _ctx.ProductCategories.Any(pc => pc.Description == entity.Description);
                } 
                else
                {
                    ProductCategory productCategory = _ctx.ProductCategories.FirstOrDefault(pc => pc.Description == entity.Description);
                    if(productCategory != null)
                    {
                        if(productCategory.Id == entity.Id)
                        {
                            return (productCategory.Description != entity.Description);
                        }
                        return true;
                    }
                    return false;
                }
            }

            return false;
        }
    }
}
