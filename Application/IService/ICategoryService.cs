using Application.ServiceResponse;
using Application.ViewModels.UserDTO;

namespace Application.IService;

public interface ICategoryService
{
    public Task<ServiceResponse<PaginationModel<CategoryResDTO>>> GetListCategory(int page, int pageSize, string search, string sort);
    public Task<ServiceResponse<CategoryResDTO>> GetCategoryById(int categoryId);
    public Task<ServiceResponse<CategoryResDTO>> CreateCategory(CategoryReqDTO createForm);
    public Task<ServiceResponse<CategoryResDTO>> UpdateCategory(CategoryReqDTO updateCategoryReq, int categoryId);
    public Task<ServiceResponse<bool>> DeleteCategory(int categoryId);

}