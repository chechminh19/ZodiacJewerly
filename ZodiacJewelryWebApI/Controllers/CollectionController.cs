using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.CollectionsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers;

[Route("api/collections")]
[ApiController]
public class CollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    public CollectionController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    [Authorize(Roles = "Admin, Staff, Customer")]
    [HttpGet]
    public async Task<IActionResult> GetListCollection([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
        [FromQuery] string search = "", [FromQuery] string filter = "", [FromQuery] string sort = "id")
    {
        var result = await _collectionService.GetListCollections(page, pageSize, search, filter, sort);
        if (!result.Success) return BadRequest(result);
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

    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromForm] CollectionsReqDTO form)
    {
        var result = await _collectionService.CreateCollection(form);

        if (!result.Success) return BadRequest(result);
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

    [Authorize(Roles = "Staff")]
    [HttpPost("{collectionId}/products/{productId}")]
    public async Task<IActionResult> AddProductToCollection(int collectionId, int productId)
    {
        var result = await _collectionService.AddProductToCollection(collectionId, productId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Admin, Staff, Customer")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCollectionDetails(int id)
    {
        var result = await _collectionService.GetCollectionDetails(id);
        if (!result.Success) return BadRequest(result);
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
                ImageUrls = p.ImageUrls.Select(iu => new { iu.Id, iu.ImageUrl }).ToList(),
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

    [Authorize(Roles = "Staff")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(int id, [FromForm] CollectionsReqDTO form)
    {
        var result = await _collectionService.UpdateCollection(form, id);
        if (!result.Success) return BadRequest(result);
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

    [Authorize(Roles = "Staff")]
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] CollectionStatusReqDTO statusReqDto)
    {
        var result = await _collectionService.ChangeStatusCollection(id, statusReqDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        var result = await _collectionService.DeleteCollection(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}