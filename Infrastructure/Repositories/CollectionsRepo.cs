using Application.Enums;
using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollectionsRepo : GenericRepo<Collections>, ICollectionRepo
{
    private readonly AppDbContext _context;

    public CollectionsRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Collections>> GetCollections()
    {
        return await _context.Collection.ToListAsync();
    }

    public async Task<Collections?> GetCollectionById(int collectionId)
    {
        return await _context.Collection.FirstOrDefaultAsync(c => c.Id == collectionId);
    }

    public async Task<Collections?> GetCollectionByName(string collectionName)
    {
        return await _context.Collection.FirstOrDefaultAsync(c => c.NameCollection == collectionName);
    }

    public async Task<Collections?> GetCollectionDetails(int collectionId)
    {
        return await _context.Collection
            .Include(c => c.CollectionProducts)
            .ThenInclude(cp => cp.Product).ThenInclude(product => product.ProductImages)
            .FirstOrDefaultAsync(c => c.Id == collectionId);
    }
}