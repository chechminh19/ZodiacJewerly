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

    public async Task<ServiceResponse<PaginationModel<CollectionsResDTO>>> GetListCollections(int page)
    {
        var result = new ServiceResponse<PaginationModel<CollectionsResDTO>>();
        try
        {
            if (page <= 0)
            {
                page = 1;
            }

            var collection = await _collectionRepo.GetCollections();
            List<CollectionsResDTO> collectionList = [];
            foreach (var c in collection)
            {
                CollectionsResDTO cr = new()
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
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<CollectionsResDTO>> GetCollectionById(int collectionId)
    {
        var result = new ServiceResponse<CollectionsResDTO>();
        try
        {
            var collection = await _collectionRepo.GetCollectionById(collectionId);
            if (collection is null)
            {
                result.Success = false;
                result.Message = "Collection not found";
            }
            else
            {
                var resCollection = _mapper.Map<Collections, CollectionsResDTO>(collection);

                result.Data = resCollection;
                result.Success = true;
            }
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<int>> CreateCollection(CollectionsReqDTO creatForm)
    {
        var result = new ServiceResponse<int>();
        try
        {
            var collectionExist = await _collectionRepo.GetCollectionByName(creatForm.NameCollection);
            if (collectionExist != null)
            {
                result.Success = false;
                result.Message = "Collection with the same name already exist";
            }
            else
            {
                var newCollection = _mapper.Map<CollectionsReqDTO, Collections>(creatForm);
                newCollection.Id = 0;
                await _collectionRepo.AddAsync(newCollection);

                result.Data = newCollection.Id;
                result.Success = true;
                result.Message = "Collections created successfully";
            }
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<string>> UpdateCollection(CollectionsReqDTO updateForm, int collectionId)
    {
        var result = new ServiceResponse<string>();
        try
        {
            ArgumentNullException.ThrowIfNull(updateForm);

            var collectionUpdate = await _collectionRepo.GetCollectionById(collectionId) ??
                                   throw new ArgumentException("Given Collection Id does not exist");
            collectionUpdate.NameCollection = updateForm.NameCollection;
            collectionUpdate.ImageCollection = updateForm.ImageCollection;
            collectionUpdate.DateOpen = updateForm.DateOpen;
            collectionUpdate.DateClose = updateForm.DateClose;

            await _collectionRepo.Update(collectionUpdate);

            result.Success = true;
            result.Message = "Collection updated successfully";
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<string>> DeleteCollection(int collectionId)
    {
        var result = new ServiceResponse<string>();
        try
        {
            var collectionExist = await _collectionRepo.GetCollectionById(collectionId);
            if (collectionExist == null)
            {
                result.Success = false;
                result.Message = "Collection not found";
            }
            else
            {
                await _collectionRepo.Remove(collectionExist);
            }

            result.Success = true;
            result.Message = "Delete successfully";
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }
}