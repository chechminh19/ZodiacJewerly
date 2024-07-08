using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollectionProductRepo : GenericRepo<CollectionProduct>, ICollectionProductRepo
{
    private readonly AppDbContext _context;

    public CollectionProductRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task AddProductToCollectionAsync(int collectionId, int productId)
    {
        var collectionProduct = new CollectionProduct
        {
            CollectionId = collectionId,
            ProductId = productId
        };

        await _context.Set<CollectionProduct>().AddAsync(collectionProduct);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ProductExistsInCollectionAsync(int collectionId, int productId)
    {
        return await _context.CollectionProduct
            .AnyAsync(cp => cp.CollectionId == collectionId && cp.ProductId == productId);
    }
}