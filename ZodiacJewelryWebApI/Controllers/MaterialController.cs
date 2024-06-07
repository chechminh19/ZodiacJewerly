using Application.IService;
using Application.ViewModels.MaterialDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers;

[Route("api/material-management")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MaterialController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [Authorize(Roles = "Staff")]
    [HttpGet("materials")]
    public async Task<IActionResult> GetMaterials()
    {
        var result = await _materialService.GetAllMaterials();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPost("material")]
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
    [HttpGet("materials/{id}")]
    public async Task<IActionResult> GetMaterialById(int id)
    {
        var result = await _materialService.GetMaterialById(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("materials/{id}")]
    public async Task<IActionResult> EditMaterial(int id, [FromBody] MaterialReqDTO form)
    {
        var result = await _materialService.UpdateMaterial(form, id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("materials/{id}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var result = await _materialService.DeleteMaterial(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}