using Application.ViewModels;
using Application.ViewModels.CollectionProductDTO;
using Domain.Entities;

namespace Application.IRepositories;

public interface ICollectionProductRepo: IGenericRepo<CollectionProduct>
{
    Task AddProductToCollectionAsync(int collectionId, int productId);

}