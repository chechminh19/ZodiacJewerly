using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ZodiacRepo : GenericRepo<Zodiac>, IZodiacRepo
    {
        private readonly AppDbContext _dbContext;

        public ZodiacRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Zodiac?> GetZodiacById(int id)
        {
            return await _dbContext.Zodiac.FindAsync(id);
        }

        public async Task<IEnumerable<Zodiac>> GetAllZodiacs()
        {
            var zodiacs = await _dbContext.Zodiac.ToListAsync();
            return zodiacs ?? new List<Zodiac>();
        }
        public async Task UpdateZodiac(Zodiac zodiac)
        {
            try
            {
                if (zodiac == null)
                {
                    throw new ArgumentNullException(nameof(zodiac));
                }

                _dbContext.Entry(zodiac).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the zodiac.", ex);
            }
        }
    }
}
