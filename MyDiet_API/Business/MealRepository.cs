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
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;

        private const string NAME_PARAMETER = "Name";

        public MealRepository(ApplicationDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public async Task<IList<MealDto>> GetAll()
        {
            return _mapper.Map<IList<Meal>, IList<MealDto>>(await _ctx.Meals.ToListAsync());
        }

        public async Task<MealDto> Get(int id)
        {
            Meal mealFromDb = await _ctx.Meals.FindAsync(id);
            return _mapper.Map<Meal, MealDto>(mealFromDb);
        }
        
        public async Task<MealDto> Create(MealDto entity)
        {
            Meal mealToAdd = _mapper.Map<MealDto, Meal>(entity);
            Meal newMeal = (await _ctx.Meals.AddAsync(mealToAdd)).Entity;
            await _ctx.SaveChangesAsync();

            return _mapper.Map<Meal, MealDto>(newMeal);
        }

        public async Task<MealDto> Update(int id, MealDto entity)
        {
            Meal mealFromDb = await _ctx.Meals.FindAsync(id);
            Meal mealToUpdate = _mapper.Map<MealDto, Meal>(entity);

            _ctx.Entry(mealFromDb).CurrentValues.SetValues(mealToUpdate);
            await _ctx.SaveChangesAsync();

            return _mapper.Map<Meal, MealDto>(mealToUpdate);
        }

        public async Task Delete(int id)
        {
            Meal mealFromDb = await _ctx.Meals.FindAsync(id);
            _ctx.Meals.Remove(mealFromDb);

            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> AddMealToDiet(int dietId, int mealId)
        {
            bool exists = await _ctx.DietMeals.AnyAsync(dm => dm.DietId == dietId && dm.MealId == mealId);
            if(!exists)
            {
                DietMeal dietMeal = new DietMeal
                {
                    MealId = mealId,
                    DietId = dietId
                };

                await _ctx.AddAsync(dietMeal);
                await _ctx.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task RemoveMealFromDiet(int dietId, int mealId)
        {
            DietMeal dietMeal = await _ctx.DietMeals.Where(dm => dm.DietId == dietId && dm.MealId == mealId).FirstOrDefaultAsync();
            _ctx.Remove(dietMeal);
            await _ctx.SaveChangesAsync();
        }

        public bool CheckIfUnique(string parameter, MealDto entity)
        {
            if(parameter.Equals(NAME_PARAMETER))
            {
                return _ctx.Meals.Any(m => m.Name == entity.Name);
            }

            return false;
        }

        public async Task<bool> AddProductToMeal(int mealId, int productId)
        {
            bool exists = await _ctx.MealProducts.AnyAsync(mp => mp.MealId == mealId && mp.ProductId == productId);
            if(!exists)
            {
                MealProduct mealProduct = new MealProduct
                {
                    MealId = mealId,
                    ProductId = productId
                };
                await _ctx.AddAsync(mealProduct);
                await _ctx.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task RemoveProductFromMeal(int mealId, int productId)
        {
            MealProduct mealProduct = await _ctx.MealProducts.FirstOrDefaultAsync(mp => mp.MealId == mealId && mp.ProductId == productId);
            _ctx.Remove(mealProduct);

            await _ctx.SaveChangesAsync();
        }
    }
}
