using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ImageRepo : GenericRepo<ProductImage>, IImageRepo
    {
        private readonly AppDbContext _dbContext;

        public ImageRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<ProductImage> GetImageInforById(int id)
        {
            return await _dbContext.ProductImage.FindAsync(id);
        }

        public async Task<IEnumerable<ProductImage>> GetAllImageInfors()
        {
            return _dbContext.ProductImage.ToList();
        }

       
    }
}
