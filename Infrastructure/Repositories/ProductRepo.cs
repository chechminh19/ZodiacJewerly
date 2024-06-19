using Application.IRepositories;
using Application.ViewModels.OrderDTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var product = await _dbContext.Product
                .Include(p => p.ProductImages)
                .Include(p => p.ProductZodiacs) 
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            return product;
        }


        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await _dbContext.Product
                .Include(p => p.ProductImages)
                .Include(p => p.ProductZodiacs)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task AddProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                await _dbContext.Product.AddAsync(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }

        public async Task UpdateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                _dbContext.Entry(product).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.Product.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found.");
                }

                _dbContext.Product.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product.", ex);
            }
        }

        public async Task<Product> GetProductByIdToOrder(int id)
        {
            var product = await _dbContext.Product
           .Include(p => p.Category)
           .Include(p => p.Material)
           .Include(p => p.Gender)
           .Include(p => p.ProductImages)
           .Include(p => p.ProductZodiacs)
           .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }
            return product;         
        }

        public double GetProductPriceById(int productId)
        {
            return _dbContext.Product.FirstOrDefault(p => p.Id == productId)?.Price ?? 0;
        }
    }
}
