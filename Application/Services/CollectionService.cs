using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.CollectionsDTO;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class CollectionService : ICollectionService
{
    private readonly ICollectionRepo _collectionRepo;
    private readonly IMapper _mapper;
    private readonly Cloudinary _cloudinary;

    public CollectionService(ICollectionRepo collectionRepo, IMapper mapper, Cloudinary cloudinary)
    {
        _collectionRepo = collectionRepo;
        _mapper = mapper;
        _cloudinary = cloudinary;
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

    public async Task<ServiceResponse<CollectionsResDTO>> CreateCollection(CollectionsReqDTO createForm)
    {
        var result = new ServiceResponse<CollectionsResDTO>();
        try
        {
            var collectionExist = await _collectionRepo.GetCollectionByName(createForm.NameCollection);
            if (collectionExist != null)
            {
                result.Success = false;
                result.Message = "Collection with the same name already exist!";
            }
            else
            {
                var imageURl = await UploadImageCollection(createForm.ImageCollection);

                var collection = new Collections
                {
                  NameCollection = createForm.NameCollection,
                  ImageCollection = imageURl,
                  DateOpen = DateTime.Now,
                  DateClose = createForm.DateClose,
                  Status  = 1
                };
                 await _collectionRepo.AddAsync(collection);

                 result.Data = new CollectionsResDTO
                 {
                     Id = collection.Id,
                     NameCollection = collection.NameCollection,
                     ImageCollection = collection.ImageCollection,
                     DateOpen = collection.DateOpen,
                     DateClose = collection.DateClose,
                     Status = collection.Status
                 };
                 result.Success = true;
                 result.Message = "Create Collection successfully!";
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


    public async Task<string> UploadImageCollection(IFormFile file)
    {
        if (file.Length > 0)
        {
            using (var stream = file.OpenReadStream())
            {
                var upLoadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Crop("fill").Gravity("face")
                };
                var uploadResult = await _cloudinary.UploadAsync(upLoadParams);

                if (uploadResult.Url != null)
                {
                    return uploadResult.Url.ToString();
                }
            }
        }

        throw new Exception("Failed to upload image");
    }

    public async Task<ServiceResponse<CollectionsResDTO>> UpdateCollection(CollectionsReqDTO updateForm, int collectionId)
    {
        var result = new ServiceResponse<CollectionsResDTO>();
        try
        {

            var collectionUpdate = await _collectionRepo.GetCollectionById(collectionId) ??
                                   throw new ArgumentException("Given Collection Id does not exist");
            collectionUpdate.NameCollection = updateForm.NameCollection;
            collectionUpdate.DateClose = updateForm.DateClose;

            if (updateForm.ImageCollection != null)
            {
                var imageUrl = await UploadImageCollection(updateForm.ImageCollection);
                collectionUpdate.ImageCollection = imageUrl;
            }

            await _collectionRepo.Update(collectionUpdate);

            result.Data = new CollectionsResDTO
            {
                Id = collectionUpdate.Id,
                NameCollection = collectionUpdate.NameCollection,
                ImageCollection = collectionUpdate.ImageCollection,
                DateOpen = collectionUpdate.DateOpen,
                DateClose = collectionUpdate.DateClose,
                Status = collectionUpdate.Status
            };
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

    public async Task<ServiceResponse<CollectionsResDTO>> ChangeStatusCollection(int collectionId, CollectionStatusReqDTO statusReq)
    {
        var result = new ServiceResponse<CollectionsResDTO>();
        try
        {
            var collection = await _collectionRepo.GetCollectionById(collectionId);
            if (collection == null)
            {
                result.Success = false;
                result.Message = "Collection not found";
                return result;
            }

            collection.Status = statusReq.Status;
            await _collectionRepo.Update(collection);

            result.Success = true;
            result.Data = new CollectionsResDTO
            {
                Id = collection.Id,
                NameCollection = collection.NameCollection,
                ImageCollection = collection.ImageCollection,
                DateOpen = collection.DateOpen,
                DateClose = collection.DateClose,
                Status = collection.Status
            };
            result.Message = "Status change successfully!";
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