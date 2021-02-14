using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDiet_API.Business.IRepository
{
    public interface IRepository<T>
    {
        public Task<IList<T>> GetAll();
        public Task<T> Get(int id);
        public Task<T> Create(T entity);
        public Task<T> Update(int id, T entity);
        public Task Delete(int id);
        public bool CheckIfUnique(string parameter, T entity);
    }
}
