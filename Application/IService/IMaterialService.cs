using Application.ServiceResponse;
using Application.ViewModels.MaterialDTO;

namespace Application.IService;

public interface IMaterialService
{
    public Task<ServiceResponse<MaterialResDTO>> GetAllMaterials();
    public Task<ServiceResponse<MaterialResDTO>> GetMaterialById(int materialId);
    public Task<ServiceResponse<int>> CreateMaterial(MaterialReqDTO createForm);
    public Task<ServiceResponse<string>> UpdateMaterial(MaterialReqDTO updateForm, int materialId);
    public Task<ServiceResponse<string>> DeleteMaterial(int materialId);
}