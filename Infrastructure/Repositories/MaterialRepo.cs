using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MaterialRepo : GenericRepo<Material>, IMaterialRepo
{
    private readonly AppDbContext _context;
    public MaterialRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Material>> GetAllMaterials()
    {
        return await _context.Material.ToListAsync();
    }

    public async Task<Material?> GetMaterialById(int materialId)
    {
        return await _context.Material.Where(m => m.Id == materialId).FirstOrDefaultAsync();
    }

    public async Task<Material?> GetMaterialByName(string materialName)
    {
        return await _context.Material.Where(m => m.NameMaterial == materialName).FirstOrDefaultAsync();
    }
}