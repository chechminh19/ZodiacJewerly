using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        private readonly AppDbContext _dbContext;

        public ProductRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Product.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await _dbContext.Product.ToListAsync();
        }

        public async Task AddProduct(Product product)
        {
            _dbContext.Product.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _dbContext.Product.FindAsync(id);
            if (product != null)
            {
                _dbContext.Product.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
