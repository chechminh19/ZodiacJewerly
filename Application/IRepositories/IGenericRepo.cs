using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IGenericRepo<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        //void UpdateRange(List<T> entities);
        //void SoftRemove(T entity);
        //Task AddRangeAsync(List<T> entities);
        //void SoftRemoveRange(List<T> entities);
    }
}
