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
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<MealRepository> _logger;

        private const string NAME_PARAMETER = "Name";

        public MealRepository(ApplicationDbContext ctx, IMapper mapper, ILogger<MealRepository> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<MealDto>> GetAllAsync()
        {
            _logger.LogInformation("Entered in GetAllAsync");
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            List<Meal> meals = await _ctx.Meals.ToListAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<IList<Meal>, IList<MealDto>>(meals);
        }

        public async Task<MealDto> GetAsync(int id)
        {
            _logger.LogInformation("Entered in Get with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Meal mealFromDb = await _ctx.Meals.FindAsync(id);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

            if (mealFromDb == null)
            {
                _logger.LogError("Meal with id {} not found", id);
                return null;
            }

            return _mapper.Map<Meal, MealDto>(mealFromDb);
        }

        public async Task<MealDto> CreateAsync(MealDto entity)
        {
            _logger.LogInformation("Entered in Create with mealDto {}", entity);
            Stopwatch stopWatch = new Stopwatch();

            Meal mealToAdd = _mapper.Map<MealDto, Meal>(entity);

            stopWatch.Start();

            Meal newMeal = (await _ctx.Meals.AddAsync(mealToAdd)).Entity;
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Meal, MealDto>(newMeal);
        }

        public async Task<MealDto> UpdateAsync(int id, MealDto entity)
        {
            _logger.LogInformation("Entered in Update with id {}, mealDto {}", id, entity);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Meal mealFromDb = await _ctx.Meals.FindAsync(id);

            if (mealFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Meal with id {} not found", id);

                return null;
            }

            Meal mealToUpdate = _mapper.Map<MealDto, Meal>(entity);

            _ctx.Entry(mealFromDb).CurrentValues.SetValues(mealToUpdate);
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Meal, MealDto>(mealToUpdate);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Entered in Delete with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Meal mealFromDb = await _ctx.Meals.FindAsync(id);
            if (mealFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Meal with id {} not found", id);

                return false;
            }
            _ctx.Meals.Remove(mealFromDb);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            await _ctx.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddMealToDietAsync(int dietId, int mealId)
        {
            _logger.LogInformation("Entered in AddMealToDiet with dietId {}, mealId {}", dietId, mealId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            bool exists = await _ctx.DietMeals.AnyAsync(dm => dm.DietId == dietId && dm.MealId == mealId);
            if (!exists)
            {
                DietMeal dietMeal = new DietMeal
                {
                    MealId = mealId,
                    DietId = dietId
                };

                await _ctx.AddAsync(dietMeal);
                await _ctx.SaveChangesAsync();

                stopWatch.Stop();

                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
                return true;
            }

            _logger.LogWarning("Meal with id {} is already present in the diet {}", mealId, dietId);

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return false;
        }

        public async Task<bool> RemoveMealFromDietAsync(int dietId, int mealId)
        {
            _logger.LogInformation("Entered in RemoveMealFromDiet with dietId {}, mealId {}", dietId, mealId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            DietMeal dietMeal = await _ctx.DietMeals.Where(dm => dm.DietId == dietId && dm.MealId == mealId).FirstOrDefaultAsync();
            
            if(dietMeal == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("No Meal with id {} is associated with diet {}", mealId, dietId);

                return false;
            }

            _ctx.Remove(dietMeal);
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

            return true;
        }

        public async Task<bool> AddProductToMealAsync(int mealId, int productId)
        {
            _logger.LogInformation("Entered in AddProductToMeal with mealId {}, productId {}", mealId, productId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            bool exists = await _ctx.MealProducts.AnyAsync(mp => mp.MealId == mealId && mp.ProductId == productId);
            if (!exists)
            {
                MealProduct mealProduct = new MealProduct
                {
                    MealId = mealId,
                    ProductId = productId
                };
                await _ctx.AddAsync(mealProduct);
                await _ctx.SaveChangesAsync();

                stopWatch.Stop();

                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                return true;
            }

            _logger.LogWarning("Product with id {} is already present in the meal {}", productId, mealId);
            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return false;
        }

        public async Task<bool> RemoveProductFromMealAsync(int mealId, int productId)
        {
            _logger.LogInformation("Entered in RemoveProductFromMeal with mealId {}, productId {}", mealId, productId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            MealProduct mealProduct = await _ctx.MealProducts.FirstOrDefaultAsync(mp => mp.MealId == mealId && mp.ProductId == productId);
            
            if(mealProduct == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("No product with id {} is associated with meal {}", productId, mealId);

                return false;
            }
            _ctx.Remove(mealProduct);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

            return true;
        }

        public bool CheckIfUnique(string parameter, MealDto entity)
        {
            if (parameter.Equals(NAME_PARAMETER))
            {
                return _ctx.Meals.Any(m => m.Name == entity.Name);
            }

            return false;
        }
    }
}
