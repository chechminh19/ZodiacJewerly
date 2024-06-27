using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.ZodiacDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IZodiacService
    {
        Task<ServiceResponse<PaginationModel<ZodiacDTO>>> GetAllZodiacs(int page, int pageSize, string search, string sort);
        Task<ServiceResponse<ZodiacDTO>> GetZodiacById(int id);
        Task<ServiceResponse<string>> UpdateZodiac(ZodiacUpdateDTO zodiac);
    }
}
