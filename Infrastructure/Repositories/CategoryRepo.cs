using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
{
    private readonly DbSet<Category> _categories;
    
    public CategoryRepo(AppDbContext context) : base(context)
    {
        _categories = context.Set<Category>();
    }

    public async Task<List<Category>> GetListCategory()
    {
        return await _categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int categoryId)
    {
        return await _categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
    }

    public async Task<Category?> GetCategoryByName(string categoryName)
    {
        return await _categories.Where(c => c.NameCategory == categoryName).FirstOrDefaultAsync();
    }
}