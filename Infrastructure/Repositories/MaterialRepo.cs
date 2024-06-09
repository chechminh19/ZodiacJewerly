using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MaterialRepo : GenericRepo<Material>, IMaterialRepo
{
    private readonly DbSet<Material> _materials;
    public MaterialRepo(AppDbContext context) : base(context)
    {
        _materials = context.Set<Material>();
    }

    public async Task<List<Material>> GetAllMaterials()
    {
        return await _materials.ToListAsync();
    }

    public async Task<Material?> GetMaterialById(int materialId)
    {
        return await _materials.Where(m => m.Id == materialId).FirstOrDefaultAsync();
    }

    public async Task<Material?> GetMaterialByName(string materialName)
    {
        return await _materials.Where(m => m.NameMaterial == materialName).FirstOrDefaultAsync();
    }
}