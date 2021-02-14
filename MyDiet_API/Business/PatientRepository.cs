using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyDiet_API.Business.IRepository;
using MyDiet_API.Data;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Business
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IMapper _mapper;

        private const string FISCAL_CODE_PARAMETER = "FiscalCode";
        private const string NAME_PARAMETER = "Name";
        private const string SURNAME_PARAMETER = "Surname";
        private const string PHONE_PARAMETER = "Phone";
        private const string EMAIL_PARAMETER = "Email";

        public PatientRepository(ApplicationDbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<IList<PatientDto>> GetAll()
        {
            return _mapper.Map<IList<Patient>, IList<PatientDto>>(await _ctx.Patients.ToListAsync());
        }

        public async Task<PatientDto> Get(int id)
        {
            Patient patient = await _ctx.Patients.FindAsync(id);
            Weight patientWeight = await _ctx.Weights.FirstOrDefaultAsync(w => w.PatientId == patient.Id);
            patient.Weight = patientWeight.WeightValue;

            return _mapper.Map<Patient, PatientDto>(patient);
        }

        public async Task<PatientDto> Create(PatientDto patientDto)
        {
            Patient patient = _mapper.Map<PatientDto, Patient>(patientDto);
            Patient newPatient = (await _ctx.Patients.AddAsync(patient)).Entity;
            await _ctx.SaveChangesAsync();

            await _ctx.Weights.AddAsync(new Weight
            {
                PatientId = patient.Id,
                Date = DateTime.Now,
                WeightValue = patient.Weight
            });

            await _ctx.SaveChangesAsync();

            return _mapper.Map<Patient, PatientDto>(newPatient);
        }

        public async Task<PatientDto> Update(int id, PatientDto patientDto)
        {
            Patient patientFromDb = await _ctx.Patients.Include(p => p.Diet).FirstOrDefaultAsync(p => p.Id == id);
            Patient patientToUpdate = _mapper.Map<PatientDto, Patient>(patientDto, patientFromDb);
            _ctx.Entry(patientFromDb).CurrentValues.SetValues(patientToUpdate);

            await _ctx.SaveChangesAsync();

            return _mapper.Map<Patient, PatientDto>(patientToUpdate);
        }

        public async Task Delete(int id)
        {
            Patient patient = await _ctx.Patients.FindAsync(id);
            _ctx.Patients.Remove(patient);

            await _ctx.SaveChangesAsync();
        }

        public async Task DisassociateDiet(int id)
        {
            Patient patient = await _ctx.Patients.Include(p => p.Diet).FirstOrDefaultAsync(p => p.DietId == id);
            if(patient != null)
            {
                patient.Diet = null;
                await _ctx.SaveChangesAsync();
            }
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
