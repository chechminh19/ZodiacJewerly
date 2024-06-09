using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<CategoryResDTO>> GetCategoryById(int categoryId)
    {
        var result = new ServiceResponse<CategoryResDTO>();
        try
        {
            var category = await _categoryRepo.GetCategoryById(categoryId);
            if (category == null)
            {
                result.Success = false;
                result.Message = "Category not found";
            }
            else
            {
                var resCategory = _mapper.Map<Category, CategoryResDTO>(category);

                result.Data = resCategory;
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

    public async Task<ServiceResponse<CategoryResDTO>> GetListCategory()
    {
        var result = new ServiceResponse<CategoryResDTO>();
        try
        {
            var category = await _categoryRepo.GetListCategory();
            List<CategoryResDTO> categoryList = [];
            categoryList.AddRange(
                category.Select(c => new CategoryResDTO() { Id = c.Id, NameCategory = c.NameCategory }));
            result.Data = new CategoryResDTO();
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

    public async Task<ServiceResponse<int>> CreateCategory(CategoryReqDTO createForm)
    {
        var result = new ServiceResponse<int>();
        try
        {
            var categoryExist = await _categoryRepo.GetCategoryByName(createForm.NameCategory);
            if (categoryExist != null)
            {
                result.Success = false;
                result.Message = "Category with the same name already exist!";
            }
            else
            {
                var newCategory = _mapper.Map<CategoryReqDTO, Category>(createForm);
                newCategory.Id = 0;
                await _categoryRepo.AddAsync(newCategory);
                result.Data = newCategory.Id;
                result.Success = true;
                result.Message = "Category created successfully!";
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

    public async Task<ServiceResponse<string>> UpdateCategory(CategoryReqDTO updateCategoryReq, int categoryId)
    {
        var result = new ServiceResponse<string>();
        try
        {
            ArgumentNullException.ThrowIfNull(updateCategoryReq);

            var categoryUpdate = await _categoryRepo.GetCategoryById(categoryId) ??
                                 throw new ArgumentException("Given category Id doesn't exist!");
            categoryUpdate.NameCategory = updateCategoryReq.NameCategory;

            await _categoryRepo.Update(categoryUpdate);

            result.Success = true;
            result.Message = "Update Category successfully";
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

    public async Task<ServiceResponse<string>> DeleteCategory(int categoryId)
    {
        var result = new ServiceResponse<string>();

        try
        {
            var categoryExist = await _categoryRepo.GetCategoryById(categoryId);
            if (categoryExist == null)
            {
                result.Success = false;
                result.Message = "Category not found";
            }
            else
            {
                await _categoryRepo.Remove(categoryExist);
            }

            result.Success = true;
            result.Message = "Delete successfully";
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.Message;
        }

        return result;
    }
}