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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;

        private const string DESCRIPTION_PARAMETER = "Description";

        public ProductCategoryRepository(ApplicationDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<IList<ProductCategoryDto>> GetAll()
        {
            return _mapper.Map<IList<ProductCategory>, IList<ProductCategoryDto>>(await _ctx.ProductCategories.ToListAsync());
        }

        public async Task<ProductCategoryDto> Get(int id)
        {
            ProductCategory productCategory = await _ctx.ProductCategories.FindAsync(id);
            return _mapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
        }

        public async Task<ProductCategoryDto> Create(ProductCategoryDto productCategoryDto)
        {
            ProductCategory productCategory = _mapper.Map<ProductCategoryDto, ProductCategory>(productCategoryDto);
            ProductCategory newProductCategory = (await _ctx.ProductCategories.AddAsync(productCategory)).Entity;
            
            await _ctx.SaveChangesAsync();
            return _mapper.Map<ProductCategory, ProductCategoryDto>(newProductCategory);
        }

        public async Task<ProductCategoryDto> Update(int id, ProductCategoryDto productCategoryDto)
        {
            ProductCategory productCategoryFromDb = await _ctx.ProductCategories.FindAsync(id);
            ProductCategory productCategoryToUpdate = _mapper.Map<ProductCategoryDto, ProductCategory>(productCategoryDto);
            _ctx.Entry(productCategoryFromDb).CurrentValues.SetValues(productCategoryToUpdate);
            
            await _ctx.SaveChangesAsync();

            return _mapper.Map<ProductCategory, ProductCategoryDto>(productCategoryToUpdate);
        }

        public async Task Delete(int id)
        {
            ProductCategory productCategoryFromDb = await _ctx.ProductCategories.FindAsync(id);
            _ctx.ProductCategories.Remove(productCategoryFromDb);

            await _ctx.SaveChangesAsync();
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
