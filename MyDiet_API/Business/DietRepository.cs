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
    public class DietRepository : IDietRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<DietRepository> _logger;

        private const string NAME_PARAMETER = "Name";
        public DietRepository(ApplicationDbContext ctx, IMapper mapper, IPatientRepository patientRepository, ILogger<DietRepository> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<IList<DietDto>> GetAllAsync()
        {
            _logger.LogInformation("Entered in GetAllAsync");
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            IList<Diet> diets = await _ctx.Diets.ToListAsync();
            IList<DietDto> dietsDto = new List<DietDto>();
            foreach(Diet diet in diets)
            {
                Patient patientFromDb = await _ctx.Patients.Include(p => p.Diet).FirstOrDefaultAsync(p => p.DietId == diet.Id);
                PatientDto patientDto = _mapper.Map<Patient, PatientDto>(patientFromDb);
                DietDto dietDto = _mapper.Map<Diet, DietDto>(diet);
                dietDto.PatientDto = patientDto;
                dietsDto.Add(dietDto);
            }

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return dietsDto;
        }

        public async Task<DietDto> GetAsync(int id)
        {
            _logger.LogInformation("Entered in Get with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Diet diet = await _ctx.Diets.Include(d => d.DietMeal).FirstOrDefaultAsync(d => d.Id == id);
            
            if(diet == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Diet with id {} not found", id);

                return null;
            }
            
            Patient patientFromDb = await _ctx.Patients.Include(p => p.Diet).FirstOrDefaultAsync(p => p.DietId == id);
            PatientDto patientDto = _mapper.Map<Patient, PatientDto>(patientFromDb);
            DietDto dietDto = new DietDto
            {
                PatientDto = patientDto
            };

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Diet, DietDto>(diet, dietDto);
        }

        public async Task<DietDto> CreateAsync(DietDto entity)
        {
            _logger.LogInformation("Entered in Create with dietDto {}", entity);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Diet dietToAdd = _mapper.Map<DietDto, Diet>(entity);
            Diet newDiet = (await _ctx.Diets.AddAsync(dietToAdd)).Entity;
            await _ctx.SaveChangesAsync();

            entity.PatientDto.DietId = dietToAdd.Id;
            await _patientRepository.UpdateAsync(entity.PatientDto.Id, entity.PatientDto);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Diet, DietDto>(newDiet);
        }

        public async Task<DietDto> UpdateAsync(int id, DietDto entity)
        {
            _logger.LogInformation("Entered in Update with id {}, dietDto {}", id, entity);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Diet dietFromDb = await _ctx.Diets.FindAsync(id);

            if (dietFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Diet with id {} not found", id);

                return null;
            }

            Diet dietToUpdate = _mapper.Map<DietDto, Diet>(entity);
            _ctx.Entry(dietFromDb).CurrentValues.SetValues(dietToUpdate);
            await _ctx.SaveChangesAsync();

            await _patientRepository.DisassociateDietAsync(id);
            entity.PatientDto.DietId = dietToUpdate.Id;
            await _patientRepository.UpdateAsync(entity.PatientDto.Id, entity.PatientDto);

            stopWatch.Stop();
            
            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Diet, DietDto>(dietToUpdate);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Entered in Delete with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Diet dietFromDb = await _ctx.Diets.FindAsync(id);

            if (dietFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Diet with id {} not found", id);

                return false;
            }

            _ctx.Diets.Remove(dietFromDb);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

            return true;
        }

        public async Task<DietDto> GetAllDietMealsAsync(int id)
        {
            _logger.LogInformation("Entered in GetAllDietMeals with id {}", id);
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            var diet = await _ctx.Diets.Include(d => d.DietMeal).ThenInclude(dm => dm.Meal).ThenInclude(m => m.MealProduct).ThenInclude(mp => mp.Product).FirstOrDefaultAsync(d => d.Id == id);

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Diet, DietDto>(diet);
        }

        public bool CheckIfUnique(string parameter, DietDto entity)
        {
            if(parameter.Equals(NAME_PARAMETER))
            {
                return _ctx.Diets.Any(d => d.Name == entity.Name);
            }

            return false;
        }
    }
}
