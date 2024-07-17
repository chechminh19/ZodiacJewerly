using Domain.Entities;

namespace Application.IRepositories;

public interface ICollectionRepo: IGenericRepo<Collections>
{
    Task<List<Collections>> GetCollections();
    Task<Collections?> GetCollectionById(int collectionId);
    Task<Collections?> GetCollectionByName(string collectionName);
    Task<Collections?> GetCollectionDetails(int collectionId);

}