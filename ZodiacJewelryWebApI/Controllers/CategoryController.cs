using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize(Roles = "Staff")]
    [HttpGet("categories-all")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _categoryService.GetListCategory();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPost("categories-new")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryReqDTO form)
    {
        var createForm = new CategoryReqDTO()
        {
            NameCategory = form.NameCategory
        };
        var result = await _categoryService.CreateCategory(createForm);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("categories-all/{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _categoryService.GetCategoryById(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("categories-all/{id}")]
    public async Task<IActionResult> EditCategory(int id, [FromBody] CategoryReqDTO form)
    {
        var result = await _categoryService.UpdateCategory(form, id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("categories-all/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategory(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}