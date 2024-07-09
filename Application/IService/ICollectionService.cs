using Application.ServiceResponse;
using Application.ViewModels.CollectionsDTO;
using Microsoft.AspNetCore.Http;

namespace Application.IService;

public interface ICollectionService
{
    public Task<ServiceResponse<PaginationModel<CollectionsResDTO>>> GetListCollections(int page, int pageSize, string search, string filter, string sort);
    public Task<ServiceResponse<CollectionDetailResDTO>> GetCollectionDetails(int collectionId);
    public Task<ServiceResponse<CollectionsResDTO>> CreateCollection(CollectionsReqDTO createForm);
    public Task<ServiceResponse<object>> AddProductToCollection(int collectionId, int productId);
    public Task<ServiceResponse<bool>> RemoveProductFromCollectionAsync(int collectionId, int productId);
    public Task<string> UploadImageCollection(IFormFile file);
    public Task<ServiceResponse<CollectionsResDTO>> UpdateCollection(CollectionsReqDTO updateForm, int collectionId);
    public Task<ServiceResponse<CollectionsResDTO>> ChangeStatusCollection(int collectionId, CollectionStatusReqDTO statusReq);
    public Task<ServiceResponse<string>> DeleteCollection(int collectionId);
}