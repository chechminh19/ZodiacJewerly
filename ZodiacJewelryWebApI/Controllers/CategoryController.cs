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
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] int page = 1,[FromQuery] int pageSize = 5, [FromQuery] string search = "",  [FromQuery] string sort = "id")
    {
        var result = await _categoryService.GetListCategory(page, pageSize, search, sort);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryReqDTO form)
    {
        var createForm = new CategoryReqDTO()
        {
            NameCategory = form.NameCategory
        };
        var result = await _categoryService.CreateCategory(createForm);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _categoryService.GetCategoryById(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategory(int id, [FromBody] CategoryReqDTO form)
    {
        var result = await _categoryService.UpdateCategory(form, id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategory(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}