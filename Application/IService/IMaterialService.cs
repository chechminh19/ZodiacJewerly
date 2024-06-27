using Application.ServiceResponse;
using Application.ViewModels.MaterialDTO;

namespace Application.IService;

public interface IMaterialService
{
    public Task<ServiceResponse<PaginationModel<MaterialResDTO>>> GetAllMaterials(int page, int pageSize, string search, string sort);
    public Task<ServiceResponse<MaterialResDTO>> GetMaterialById(int materialId);
    public Task<ServiceResponse<MaterialResDTO>> CreateMaterial(MaterialReqDTO createForm);
    public Task<ServiceResponse<MaterialResDTO>> UpdateMaterial(MaterialReqDTO updateForm, int materialId);
    public Task<ServiceResponse<bool>> DeleteMaterial(int materialId);
}