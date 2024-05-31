using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ZodiacProductRepo : GenericRepo<ZodiacProduct>, IZodiacProductRepo
    {
        private readonly AppDbContext _dbContext;

        public ZodiacProductRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<ZodiacProduct> GetZodiacProductById(int id)
        {
            return await _dbContext.ZodiacProduct.FindAsync(id);
        }

        public async Task<IEnumerable<ZodiacProduct>> GetAllZodiacProduct()
        {
            return await _dbContext.ZodiacProduct.ToListAsync();
        }

        public async Task AddZodiacProduct(ZodiacProduct zproduct)
        {
            _dbContext.ZodiacProduct.Add(zproduct);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateZodiacProduct(ZodiacProduct zproduct)
        {
            _dbContext.Entry(zproduct).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteZodiacProduct(int id)
        {
            var zproduct = await _dbContext.ZodiacProduct.FindAsync(id);
            if (zproduct != null)
            {
                _dbContext.ZodiacProduct.Remove(zproduct);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsByZodiacId(int zodiacId)
        {
            
            var productIds = await _dbContext.ZodiacProduct
                .Where(zp => zp.ZodiacId == zodiacId)
                .Select(zp => zp.ProductId)
                .ToListAsync();

            
            var products = await _dbContext.Product
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            return products;
        }

    }
}
