using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Business.IRepository
{
    public interface IPatientRepository : IRepository<PatientDto>
    {
        public Task DisassociateDiet(int id);
    }
}
