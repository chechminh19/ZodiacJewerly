using Application.IService;
using Application.ViewModels.MaterialDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers;

[Route("api/materials")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MaterialController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [Authorize(Roles = "Staff")]
    [HttpGet]
    public async Task<IActionResult> GetMaterials([FromQuery] int page=1, [FromQuery] int pageSize = 5, [FromQuery] string search = "",  [FromQuery] string sort = "id")
    {
        var result = await _materialService.GetAllMaterials(page, pageSize, search, sort);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterial([FromBody] MaterialReqDTO form)
    {
        var createForm = new MaterialReqDTO()
        {
            NameMaterial = form.NameMaterial
        };
        var result = await _materialService.CreateMaterial(createForm);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialById(int id)
    {
        var result = await _materialService.GetMaterialById(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditMaterial(int id, [FromBody] MaterialReqDTO form)
    {
        var result = await _materialService.UpdateMaterial(form, id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var result = await _materialService.DeleteMaterial(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}