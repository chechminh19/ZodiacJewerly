using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.CollectionsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers;

[Route("api/collections")]
[ApiController]
[Authorize(Roles = "Staff")]
public class CollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    public CollectionController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    /// <summary>
    /// Gets a paginated list of collections.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetListCollection([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
        [FromQuery] string search = "", [FromQuery] string status = "", [FromQuery] string sort = "id")
    {
        var result = await _collectionService.GetListCollections(page, pageSize, search, status, sort);
        if (!result.Success) return BadRequest(result.Message);
        var formattedData = result.Data.ListData.Select(c => new
        {
            c.Id,
            c.NameCollection,
            c.ImageCollection,
            DateOpen = c.DateOpen.ToString("f"),
            DateClose = c.DateClose.ToString("f"),
            c.Status
        }).ToList();

        var formattedResult = new ServiceResponse<object>()
        {
            Data = new
            {
                result.Data.Page,
                result.Data.TotalPage,
                result.Data.TotalRecords,
                Data = formattedData
            },
            Success = result.Success,
        };
        return Ok(formattedResult);
    }


    /// <summary>
    /// Gets the details of a collection by its ID.
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCollectionDetails(int id)
    {
        var result = await _collectionService.GetCollectionDetails(id);
        if (!result.Success) return BadRequest(result.Message);
        var formattedData = new
        {
            result.Data.Id,
            result.Data.NameCollection,
            result.Data.ImageCollection,
            DateOpen = result.Data.DateOpen.ToString("f"),
            DateClose = result.Data.DateClose.ToString("f"),
            Products = result.Data.Products.Select(p => new
            {
                p.Id,
                p.NameProduct,
                p.DescriptionProduct,
                ImageUrls = p.ImageUrls.Select(iu => iu.ImageUrl).ToList(),
                p.Price
            }).ToList()
        };

        var formattedResult = new ServiceResponse<object>()
        {
            Data = formattedData,
            Success = result.Success
        };

        return Ok(formattedResult);
    }

    /// <summary>
    /// Creates a new collection.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromForm] CollectionsReqDTO form)
    {
        var result = await _collectionService.CreateCollection(form);

        if (!result.Success) return BadRequest(result.Message);
        var formattedData = new
        {
            result.Data.Id,
            result.Data.NameCollection,
            result.Data.ImageCollection,
            DateOpen = result.Data.DateOpen.ToString("f"),
            DateClose = result.Data.DateClose.ToString("f"),
            result.Data.Status
        };

        var formattedResult = new ServiceResponse<object>
        {
            Data = formattedData,
            Success = result.Success,
            Message = result.Message,
        };
        return Ok(formattedResult);
    }


    /// <summary>
    /// Adds a product to a collection.
    /// </summary>
    [HttpPost("{collectionId}/products/{productId}")]
    public async Task<IActionResult> AddProductToCollection(int collectionId, int productId)
    {
        var result = await _collectionService.AddProductToCollection(collectionId, productId);
        return result.Success ? Ok(result) : BadRequest(result.Message);
    }


    /// <summary>
    /// Updates an existing collection.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(int id, [FromForm] CollectionsReqDTO form)
    {
        var result = await _collectionService.UpdateCollection(form, id);
        if (!result.Success) return BadRequest(result.Message);
        var formattedResult = new ServiceResponse<object>()
        {
            Data = new
            {
                result.Data.Id,
                result.Data.NameCollection,
                result.Data.ImageCollection,
                Date = result.Data.DateOpen.ToString("f")
            },
            Success = result.Success
        };
        return Ok(formattedResult);
    }

    /// <summary>
    /// Changes the status of a collection.
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] CollectionStatusReqDTO statusReqDto)
    {
        var result = await _collectionService.ChangeStatusCollection(id, statusReqDto);
        return result.Success ? Ok(result) : BadRequest(result.Message);
    }


    /// <summary>
    /// Removes a product from a collection.
    /// </summary>
    [HttpDelete("remove-product/{collectionId}/{productId}")]
    public async Task<IActionResult> RemoveProduct(int collectionId, int productId)
    {
        var result = await _collectionService.RemoveProductFromCollectionAsync(collectionId, productId);
        return result.Success ? Ok(result) : BadRequest(result.Message);
    }


    /// <summary>
    /// Deletes a collection by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        var result = await _collectionService.DeleteCollection(id);
        return result.Success ? Ok(result) : BadRequest(result.Message);
    }
}