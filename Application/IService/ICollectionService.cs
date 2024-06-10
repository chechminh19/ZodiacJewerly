using Application.ServiceResponse;
using Application.ViewModels.CollectionsDTO;

namespace Application.IService;

public interface ICollectionService
{
    public Task<ServiceResponse<PaginationModel<CollectionsRes>>> GetListCollections(int page);
}