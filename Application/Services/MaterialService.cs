using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.MaterialDTO;
using AutoMapper;
using CloudinaryDotNet.Actions;
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

    public async Task<ServiceResponse<PaginationModel<MaterialResDTO>>> GetAllMaterials(int page)
    {
        var result = new ServiceResponse<PaginationModel<MaterialResDTO>>();
        try
        {
            if (page <= 0)
            {
                page = 1;
            }
            var material = await _materialRepo.GetAllMaterials();
            List<MaterialResDTO> materialList = new List<MaterialResDTO>();
            materialList.AddRange(material.Select(m => new MaterialResDTO()
                { Id = m.Id, NameMaterial = m.NameMaterial }));

            var resultList = await Pagination.GetPagination(materialList, page, 5);
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

    public async Task<ServiceResponse<int>> CreateMaterial(MaterialReqDTO createForm)
    {
        var result = new ServiceResponse<int>();
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
                result.Data = newMaterial.Id;
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

    public async Task<ServiceResponse<string>> UpdateMaterial(MaterialReqDTO updateForm, int materialId)
    {
        var result = new ServiceResponse<string>();
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

    public async Task<ServiceResponse<string>> DeleteMaterial(int materialId)
    {
        var result = new ServiceResponse<string>();

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