using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Business.IRepository
{
    public interface IRepository<T>
    {
        public Task<IList<T>> GetAllAsync();
        public Task<T> GetAsync(int id);
        public Task<T> CreateAsync(T entity);
        public Task<T> UpdateAsync(int id, T entity);
        public Task<bool> DeleteAsync(int id);
        public bool CheckIfUnique(string parameter, T entity);
    }
}
