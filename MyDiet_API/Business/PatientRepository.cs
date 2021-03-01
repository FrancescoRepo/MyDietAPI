using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyDiet_API.Business.IRepository;
using MyDiet_API.Data;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Business
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientRepository> _logger;

        private const string FISCAL_CODE_PARAMETER = "FiscalCode";
        private const string NAME_PARAMETER = "Name";
        private const string SURNAME_PARAMETER = "Surname";
        private const string PHONE_PARAMETER = "Phone";
        private const string EMAIL_PARAMETER = "Email";

        public PatientRepository(ApplicationDbContext ctx, IMapper mapper, ILogger<PatientRepository> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<PatientDto>> GetAllAsync()
        {
            _logger.LogInformation("Entered in GetAllAsync");
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Start();
            
            List<Patient> patients = await _ctx.Patients.ToListAsync();
            
            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<IList<Patient>, IList<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetAsync(int id)
        {
            _logger.LogInformation("Entered in Get with id {}", id);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Patient patient = await _ctx.Patients.FindAsync(id);
            
            if (patient == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
                
                _logger.LogError("Patient with id {} not found", id);

                return null;
            }

            Weight patientWeight = await _ctx.Weights.FirstOrDefaultAsync(w => w.PatientId == patient.Id);
            patient.Weight = patientWeight.WeightValue;
            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Patient, PatientDto>(patient);
        }

        public async Task<PatientDto> CreateAsync(PatientDto patientDto)
        {
            _logger.LogInformation("Entered in Create with patientDto {}", patientDto);
            Stopwatch stopWatch = new Stopwatch();
            Patient patient = _mapper.Map<PatientDto, Patient>(patientDto);
            
            stopWatch.Start();
            
            Patient newPatient = (await _ctx.Patients.AddAsync(patient)).Entity;
            await _ctx.SaveChangesAsync();

            await _ctx.Weights.AddAsync(new Weight
            {
                PatientId = patient.Id,
                Date = DateTime.Now,
                WeightValue = patient.Weight
            });

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Patient, PatientDto>(newPatient);
        }

        public async Task<PatientDto> UpdateAsync(int id, PatientDto patientDto)
        {
            _logger.LogInformation("Entered in Update with id {}, patientDto {}", id, patientDto);
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Start();
            
            Patient patientFromDb = await _ctx.Patients.Include(p => p.Diet).FirstOrDefaultAsync(p => p.Id == id);
            
            if (patientFromDb == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Patient with id {} not found", id);
                
                return null;
            }

            Patient patientToUpdate = _mapper.Map<PatientDto, Patient>(patientDto, patientFromDb);
            _ctx.Entry(patientFromDb).CurrentValues.SetValues(patientToUpdate);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();

            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);
            return _mapper.Map<Patient, PatientDto>(patientToUpdate);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Entered in Delete with id {}", id);
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Start();
            
            Patient patient = await _ctx.Patients.FindAsync(id);
            
            if(patient == null)
            {
                stopWatch.Stop();
                _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

                _logger.LogError("Patient with id {} not found", id);
                
                return false;
            }

            _ctx.Patients.Remove(patient);

            await _ctx.SaveChangesAsync();

            stopWatch.Stop();
            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

            return true;
        }

        public async Task<bool> DisassociateDietAsync(int id)
        {
            _logger.LogInformation("Entered in DisassociateDiet with id {}", id);
            bool disassociated = false;
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            Patient patient = await _ctx.Patients.Include(p => p.Diet).FirstOrDefaultAsync(p => p.DietId == id);
            if(patient != null)
            {
                patient.Diet = null;
                await _ctx.SaveChangesAsync();
                disassociated = true;
            }

            stopWatch.Stop();
            _logger.LogInformation("Elapsed time in {0} ms", stopWatch.ElapsedMilliseconds);

            return disassociated;
        }

        public bool CheckIfUnique(string parameter, PatientDto entity)
        {
            bool isUnique = false;
            switch(parameter)
            {
                case FISCAL_CODE_PARAMETER:
                    isUnique = _ctx.Patients.Any(p => p.FiscalCode == entity.FiscalCode);
                    break;
                case NAME_PARAMETER:
                    isUnique = _ctx.Patients.Any(p => p.Name == entity.Name);
                    break;
                case SURNAME_PARAMETER:
                    isUnique = _ctx.Patients.Any(p => p.Surname == entity.Surname);
                    break;
                case PHONE_PARAMETER:
                    isUnique = _ctx.Patients.Any(p => p.Phone == entity.Phone);
                    break;
                case EMAIL_PARAMETER:
                    isUnique = _ctx.Patients.Any(p => p.Email == entity.Email);
                    break;
            }

            return isUnique;
        }
    }
}
