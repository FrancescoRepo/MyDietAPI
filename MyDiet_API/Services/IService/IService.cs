using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services.IService
{
    public interface IService<T>
    {
        public Task<IList<T>> GetAll();
        public Task<T> Get(int id);
        public Task<T> Create(T entityDto);
        public Task<T> Update(int id, T entityDto);
        public Task Delete(int id);
        public bool CheckIfUnique(string parameter, T entityDto);
    }
}
