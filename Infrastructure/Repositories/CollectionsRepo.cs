using Application.Enums;
using Application.IRepositories;
using Application.ViewModels.CollectionsDTO;
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
        return await _context.Collection.Where(c => c.Status == (byte)Status.Active).ToListAsync();
    }

    public async Task<Collections?> GetCollectionById(int collectionId)
    {
        return await _context.Collection.FirstOrDefaultAsync(c => c.Id == collectionId);
    }

    public async Task<Collections?> GetCollectionByName(string collectionName)
    {
        return await _context.Collection.FirstOrDefaultAsync(c => c.NameCollection == collectionName);
    }

    public async Task<CollectionDetailResDTO> GetCollectionDetails(int collectionId)
    {
        try
        {
            var collection = await _context.Collection
                .Include(c => c.CollectionProducts)
                .ThenInclude(cp => cp.Product).ThenInclude(product => product.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == collectionId);

            if (collection == null)
            {
                return null;
            }

            var collectionDetailResDto = new CollectionDetailResDTO()
            {
                Id = collection.Id,
                NameCollection = collection.NameCollection,
                ImageCollection = collection.ImageCollection,
                DateOpen = collection.DateOpen,
                DateClose = collection.DateClose,
                Products = collection.CollectionProducts.Select(cp => new ProductOfCollectionResDTO
                {
                    Id = cp.Product.Id,
                    NameProduct = cp.Product.NameProduct,
                    DescriptionProduct = cp.Product.DescriptionProduct,
                    ImageUrls = cp.Product.ProductImages.Select(pi => new ProductImageCollectionDTO
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageUrl
                    }).ToList(),
                    Price = cp.Product.Price
                }).ToList()
            };
            return collectionDetailResDto;
        }
        catch (Exception e)
        {
            throw new Exception($"Error retrieving collection details: {e.Message}", e);
        }
    }
}