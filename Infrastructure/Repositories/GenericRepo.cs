using Application.IRepositories;
using Application.IService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected DbSet<T> _dbSet;
        //private readonly ICurrentTime _timeService;
        //private readonly IClamService _clamService;

        public GenericRepo(AppDbContext context)
        {
            _dbSet = context.Set<T>();        
            
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
        //public Task<List<T>> GetAllAsync() => _dbSet.ToListAsync();

        //public async Task<T?> GetByIdAsync(Guid id)
        //{
        //    var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        //    // todo should throw exception when not found
        //    return result;
        //}

        //public async Task AddAsync(T entity)
        //{
        //    entity.CreationDate = _timeService.GetCurrentTime();

        //    await _dbSet.AddAsync(entity);
        //}

        //public void SoftRemove(T entity)
        //{
        //    entity.IsDeleted = true;
        //    _dbSet.Update(entity);
        //}

        //public void Update(T entity)
        //{

        //    _dbSet.Update(entity);
        //}

        //public async Task AddRangeAsync(List<T> entities)
        //{
        //    foreach (var entity in entities)
        //    {
        //        entity.CreationDate = _timeService.GetCurrentTime();

        //    }
        //    await _dbSet.AddRangeAsync(entities);
        //}


    }
}
