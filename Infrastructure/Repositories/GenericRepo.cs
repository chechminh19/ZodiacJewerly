﻿using Application.IRepositories;
using Application.IService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected DbSet<T> _dbSet;    
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

        public  void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);            
        }       
        public Task UpdatePropertyAsync(T entity, Expression<Func<T, object>> propertyExpression)
        {
            _dbSet.Attach(entity);
            _dbSet.Entry(entity).Property(propertyExpression).IsModified = true;
            return Task.CompletedTask;
        }

        public Task Update(T entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepo<T>.Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
