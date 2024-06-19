using Application.IService;
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

    [Authorize(Roles = "Staff")]
    [HttpGet]
    public async Task<IActionResult> GetListCollection(int page)
    {
        var result = await _collectionService.GetListCollections(page);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromForm] CollectionsReqDTO form)
    {
        var result = await _collectionService.CreateCollection(form);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPost("{collectionId}/products/{productId}")]
    public async Task<IActionResult> AddProductToCollection(int collectionId, int productId)
    {
        var result = await _collectionService.AddProductToCollection(collectionId, productId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCollectionDetails(int id)
    {
        var result = await _collectionService.GetCollectionDetails(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(int id, [FromForm] CollectionsReqDTO form)
    {
        var result = await _collectionService.UpdateCollection(form, id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] CollectionStatusReqDTO statusReqDto)
    {
        var result = await _collectionService.ChangeStatusCollection(id , statusReqDto);
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