using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.CollectionsDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class CollectionService : ICollectionService
{
    private readonly ICollectionRepo _collectionRepo;
    private readonly IMapper _mapper;

    public CollectionService(ICollectionRepo collectionRepo, IMapper mapper)
    {
        _collectionRepo = collectionRepo;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<PaginationModel<CollectionsRes>>> GetListCollections(int page)
    {
        var result = new ServiceResponse<PaginationModel<CollectionsRes>>();
        try
        {
            if (page <= 0)
            {
                page = 1;
            }

            var collection = await _collectionRepo.GetCollections();
            List<CollectionsRes> collectionList = [];
            foreach (var c in collection)
            {
                CollectionsRes cr = new()
                {
                    Id = c.Id,
                    NameCollection = c.NameCollection,
                    ImageCollection = c.ImageCollection,
                    DateOpen = c.DateOpen,
                    DateClose = c.DateClose,
                    Status = c.Status
                };
                collectionList.Add(cr);
            }

            var resultList = await Pagination.GetPagination(collectionList, page, 10);

            result.Success = true;
            result.Data = resultList;
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }
}