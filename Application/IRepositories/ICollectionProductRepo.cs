using Application.ViewModels;
using Application.ViewModels.CollectionProductDTO;
using Domain.Entities;

namespace Application.IRepositories;

public interface ICollectionProductRepo: IGenericRepo<CollectionProduct>
{
    Task<List<CollectionProductReq>> GetProductsByCollectionId(int collectionId);
    Task<List<CollectionProduct>> GetCollectionProductByCollectionId(int collectionId);
    Task<List<CollectionProduct>> GetCollectionProductByProductId(int productId);

}