using Domain.Entities;

namespace Application.IRepositories;

public interface ICollectionProductRepo: IGenericRepo<CollectionProduct>
{
    Task AddProductToCollectionAsync(int collectionId, int productId);
    Task<bool> ProductExistsInCollectionAsync(int collectionId, int productId);

}