using Application.Enums;
using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollectionsRepo : GenericRepo<Collections>, ICollectionRepo
{
    private readonly DbSet<Collections> _collections;
    public CollectionsRepo(AppDbContext context) : base(context)
    {
        _collections = context.Set<Collections>();
    }

    public async Task<List<Collections>> GetCollections()
    {
        return await _collections.Where(c => c.Status.Equals(Status.Active)).ToListAsync();
    }

    public async Task<Collections?> GetCollectionById(int collectionId)
    {
        return await _collections.FirstOrDefaultAsync(c => c.Id == collectionId);
    }

    public async Task<Collections?> GetCollectionByName(string collectionName)
    {
        return await _collections.FirstOrDefaultAsync(c => c.NameCollection == collectionName);
    }
}