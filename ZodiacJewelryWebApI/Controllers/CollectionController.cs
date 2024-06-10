using Application.IService;
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
}