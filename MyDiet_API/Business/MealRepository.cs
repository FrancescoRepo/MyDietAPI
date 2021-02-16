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

        public async Task<IList<MealDto>> GetAll()
        {
            _logger.LogInformation("Entered in GetAll");
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            
            List<Meal> meals = await _ctx.Meals.ToListAsync();
            
            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<IList<Meal>, IList<MealDto>>(meals);
        }

        public async Task<MealDto> Get(int id)
        {
            _logger.LogInformation("Entered in Get with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Meal mealFromDb = await _ctx.Meals.FindAsync(id);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Meal, MealDto>(mealFromDb);
        }
        
        public async Task<MealDto> Create(MealDto entity)
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

        public async Task<MealDto> Update(int id, MealDto entity)
        {
            _logger.LogInformation("Entered in Update with id {}, mealDto {}", id, entity);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            
            Meal mealFromDb = await _ctx.Meals.FindAsync(id);
            Meal mealToUpdate = _mapper.Map<MealDto, Meal>(entity);

            _ctx.Entry(mealFromDb).CurrentValues.SetValues(mealToUpdate);
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Meal, MealDto>(mealToUpdate);
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Entered in Delete with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Meal mealFromDb = await _ctx.Meals.FindAsync(id);
            _ctx.Meals.Remove(mealFromDb);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> AddMealToDiet(int dietId, int mealId)
        {
            _logger.LogInformation("Entered in AddMealToDiet with dietId {}, mealId {}", dietId, mealId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

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

                stopWatch.Stop();

                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
                return true;
            }

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return false;
        }

        public async Task RemoveMealFromDiet(int dietId, int mealId)
        {
            _logger.LogInformation("Entered in RemoveMealFromDiet with dietId {}, mealId {}", dietId, mealId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            DietMeal dietMeal = await _ctx.DietMeals.Where(dm => dm.DietId == dietId && dm.MealId == mealId).FirstOrDefaultAsync();
            _ctx.Remove(dietMeal);
            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
        }

        public async Task<bool> AddProductToMeal(int mealId, int productId)
        {
            _logger.LogInformation("Entered in AddProductToMeal with mealId {}, productId {}", mealId, productId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

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

                stopWatch.Stop();

                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                return true;
            }

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return false;
        }

        public async Task RemoveProductFromMeal(int mealId, int productId)
        {
            _logger.LogInformation("Entered in RemoveProductFromMeal with mealId {}, productId {}", mealId, productId);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            MealProduct mealProduct = await _ctx.MealProducts.FirstOrDefaultAsync(mp => mp.MealId == mealId && mp.ProductId == productId);
            _ctx.Remove(mealProduct);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
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
