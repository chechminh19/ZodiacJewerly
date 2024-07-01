using Application.Enums;
using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.CollectionsDTO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class CollectionService : ICollectionService
{
    private readonly ICollectionRepo _collectionRepo;
    private readonly ICollectionProductRepo _collectionProduct;
    private readonly Cloudinary _cloudinary;

    public CollectionService(ICollectionRepo collectionRepo, Cloudinary cloudinary,
        ICollectionProductRepo collectionProduct)
    {
        _collectionRepo = collectionRepo;
        _cloudinary = cloudinary;
        _collectionProduct = collectionProduct;
    }

    public async Task<ServiceResponse<PaginationModel<CollectionsResDTO>>> GetListCollections(int page, int pageSize,
        string search, string filter, string sort)
    {
        var result = new ServiceResponse<PaginationModel<CollectionsResDTO>>();
        try
        {
            if (page <= 0)
            {
                page = 1;
            }

            var collections = await _collectionRepo.GetCollections();
            if (!string.IsNullOrEmpty(search))
            {
                collections = collections
                    .Where(c => c.NameCollection.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (byte.TryParse(filter, out byte status))
            {
                collections = collections.Where(c => c.Status == status).ToList();
            }

            collections = sort.ToLower() switch
            {
                "name" => collections.OrderBy(c => c.NameCollection).ToList(),
                "dateopen" => collections.OrderBy(c => c.DateOpen).ToList(),
                "dateclose" => collections.OrderBy(c => c.DateClose).ToList(),
                "status" => collections.OrderBy(c => c.Status).ToList(),
                _ => collections.OrderBy(c => c.Id).ToList()
            };

            var collectionList = collections.Select(c => new CollectionsResDTO
            {
                Id = c.Id,
                NameCollection = c.NameCollection,
                ImageCollection = c.ImageCollection,
                DateOpen = c.DateOpen,
                DateClose = c.DateClose,
                Status = c.Status
            }).ToList();

            var resultList = await Pagination.GetPagination(collectionList, page, pageSize);

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

    public async Task<ServiceResponse<CollectionDetailResDTO>> GetCollectionDetails(int collectionId)
    {
        var result = new ServiceResponse<CollectionDetailResDTO>();
        try
        {
            var collectionDetail = await _collectionRepo.GetCollectionDetails(collectionId);

            result.Data = collectionDetail;
            result.Success = true;
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
                    Status = 1
                };
                await _collectionRepo.AddAsync(collection);

                var newCollection = new CollectionsResDTO
                {
                    Id = collection.Id,
                    NameCollection = collection.NameCollection,
                    ImageCollection = collection.ImageCollection,
                    DateOpen = collection.DateOpen,
                    DateClose = collection.DateClose,
                    Status = collection.Status
                };
                result.Data = newCollection;
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

    public async Task<ServiceResponse<object>> AddProductToCollection(int collectionId, int productId)
    {
        var result = new ServiceResponse<object>();
        try
        {
            await _collectionProduct.AddProductToCollectionAsync(collectionId, productId);
            result.Success = true;
            result.Message = "Product added to Collection successfully";
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

    public async Task<ServiceResponse<CollectionsResDTO>> UpdateCollection(CollectionsReqDTO updateForm,
        int collectionId)
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

    public async Task<ServiceResponse<CollectionsResDTO>> ChangeStatusCollection(int collectionId,
        CollectionStatusReqDTO statusReq)
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