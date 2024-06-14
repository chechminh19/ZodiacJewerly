using Application.ServiceResponse;
using Application.ViewModels.CollectionsDTO;

namespace Application.IService;

public interface ICollectionService
{
    public Task<ServiceResponse<PaginationModel<CollectionsResDTO>>> GetListCollections(int page);
    public Task<ServiceResponse<CollectionsResDTO>> GetCollectionById(int collectionId);
    public Task<ServiceResponse<int>> CreateCollection(CollectionsReqDTO creatForm);
    public Task<ServiceResponse<string>> UpdateCollection(CollectionsReqDTO updateForm, int collectionId);
    public Task<ServiceResponse<string>> DeleteCollection(int collectionId);
}
