using Domain.Entities;

namespace Application.IRepositories;

public interface IMaterialRepo : IGenericRepo<Material>
{
    Task<List<Material>> GetAllMaterials();
    Task<Material?> GetMaterialById(int materialId);
    Task<Material?> GetMaterialByName(string materialName);
}