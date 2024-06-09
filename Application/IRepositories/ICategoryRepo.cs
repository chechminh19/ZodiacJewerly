using Domain.Entities;

namespace Application.IRepositories;

public interface ICategoryRepo: IGenericRepo<Category>
{
    Task<List<Category>> GetListCategory();
    Task<Category?> GetCategoryById(int categoryId);
    Task<Category?> GetCategoryByName(string categoryName);
}