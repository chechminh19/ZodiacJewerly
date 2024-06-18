using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
{
    private readonly AppDbContext _context;
    
    public CategoryRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetListCategory()
    {
        return await _context.Category.ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int categoryId)
    {
        return await _context.Category.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
    }

    public async Task<Category?> GetCategoryByName(string categoryName)
    {
        return await _context.Category.Where(c => c.NameCategory == categoryName).FirstOrDefaultAsync();
    }
}