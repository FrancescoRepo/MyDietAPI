using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Services.IService
{
    public interface IService<T>
    {
        public Task<IList<T>> GetAllAsync();
        public Task<T> GetAsync(int id);
        public Task<T> CreateAsync(T entityDto);
        public Task<T> UpdateAsync(int id, T entityDto);
        public Task<bool> DeleteAsync(int id);
        public bool CheckIfUnique(string parameter, T entityDto);
    }
}
