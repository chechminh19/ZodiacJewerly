using Application.ServiceResponse;
using Application.ViewModels.CollectionsDTO;
using Microsoft.AspNetCore.Http;

namespace Application.IService;

public interface ICollectionService
{
    public Task<ServiceResponse<PaginationModel<CollectionsResDTO>>> GetListCollections(int page);
    public Task<ServiceResponse<CollectionsResDTO>> GetCollectionById(int collectionId);
    public Task<ServiceResponse<CollectionsResDTO>> CreateCollection(CollectionsReqDTO createForm);
    public Task<string> UploadImageCollection(IFormFile file);
    public Task<ServiceResponse<CollectionsResDTO>> UpdateCollection(CollectionsReqDTO updateForm, int collectionId);
    public Task<ServiceResponse<CollectionsResDTO>> ChangeStatusCollection(int collectionId, CollectionStatusReqDTO statusReq);
    public Task<ServiceResponse<string>> DeleteCollection(int collectionId);
}
