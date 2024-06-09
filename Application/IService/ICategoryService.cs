using Application.ServiceResponse;
using Application.ViewModels.UserDTO;

namespace Application.IService;

public interface ICategoryService
{
    public Task<ServiceResponse<CategoryResDTO>> GetListCategory();
    public Task<ServiceResponse<CategoryResDTO>> GetCategoryById(int categoryId);
    public Task<ServiceResponse<int>> CreateCategory(CategoryReqDTO createForm);
    public Task<ServiceResponse<string>> UpdateCategory(CategoryReqDTO updateCategoryReq, int categoryId);
    public Task<ServiceResponse<string>> DeleteCategory(int categoryId);

}