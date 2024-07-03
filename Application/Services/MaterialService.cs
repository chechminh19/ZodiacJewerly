using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.MaterialDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepo _materialRepo;
    private readonly IMapper _mapper;

    public MaterialService(IMapper mapper, IMaterialRepo materialRepo)
    {
        _mapper = mapper;
        _materialRepo = materialRepo;
    }

    public async Task<ServiceResponse<PaginationModel<MaterialResDTO>>> GetAllMaterials(int page, int pageSize,
        string search, string sort)
    {
        var result = new ServiceResponse<PaginationModel<MaterialResDTO>>();
        try
        {
            if (page <= 0)
            {
                page = 1;
            }

            var material = await _materialRepo.GetAllMaterials();

            if (!string.IsNullOrEmpty(search))
            {
                material = material.Where(c => c.NameMaterial.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            material = sort.ToLower() switch
            {
                "name" => material.OrderBy(c => c.NameMaterial).ToList(),
                _ => material.OrderBy(c => c.Id).ToList()
            };

            var materialList = material.Select(c => new MaterialResDTO
            {
                Id = c.Id,
                NameMaterial = c.NameMaterial
            }).ToList();

            var resultList = await Pagination.GetPagination(materialList, page, pageSize);
            result.Data = resultList;
            result.Success = true;
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<MaterialResDTO>> GetMaterialById(int materialId)
    {
        var result = new ServiceResponse<MaterialResDTO>();
        try
        {
            var material = await _materialRepo.GetMaterialById(materialId);
            if (material == null)
            {
                result.Success = false;
                result.Message = "Material not found";
            }
            else
            {
                var resMaterial = _mapper.Map<Material, MaterialResDTO>(material);

                result.Data = resMaterial;
                result.Success = true;
            }
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<MaterialResDTO>> CreateMaterial(MaterialReqDTO createForm)
    {
        var result = new ServiceResponse<MaterialResDTO>();
        try
        {
            var materialExist = await _materialRepo.GetMaterialByName(createForm.NameMaterial);
            if (materialExist != null)
            {
                result.Success = false;
                result.Message = "Material with the same name already exist!";
            }
            else
            {
                var newMaterial = _mapper.Map<MaterialReqDTO, Material>(createForm);
                newMaterial.Id = 0;
                await _materialRepo.AddAsync(newMaterial);
                result.Data = new MaterialResDTO
                {
                    Id = newMaterial.Id,
                    NameMaterial = newMaterial.NameMaterial
                };
                result.Success = true;
                result.Message = "Material created successfully!";
            }
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<MaterialResDTO>> UpdateMaterial(MaterialReqDTO updateForm, int materialId)
    {
        var result = new ServiceResponse<MaterialResDTO>();
        try
        {
            ArgumentNullException.ThrowIfNull(updateForm);

            var materialUpdate = await _materialRepo.GetMaterialById(materialId) ??
                                 throw new ArgumentException("Given material Id doesn't exist!");
            materialUpdate.NameMaterial = updateForm.NameMaterial;

            await _materialRepo.Update(materialUpdate);

            result.Success = true;
            result.Message = "Update Material successfully";
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.InnerException != null
                ? e.InnerException.Message + "\n" + e.StackTrace
                : e.Message + "\n" + e.StackTrace;
        }

        return result;
    }

    public async Task<ServiceResponse<bool>> DeleteMaterial(int materialId)
    {
        var result = new ServiceResponse<bool>();

        try
        {
            var materialExist = await _materialRepo.GetMaterialById(materialId);
            if (materialExist == null)
            {
                result.Success = false;
                result.Message = "Material not found";
            }
            else
            {
                await _materialRepo.Remove(materialExist);
            }

            result.Success = true;
            result.Message = "Delete successfully";
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.Message;
        }

        return result;
    }
}