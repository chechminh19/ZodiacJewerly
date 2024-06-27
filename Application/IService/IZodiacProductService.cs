using Application.ServiceResponse;
using Application.ViewModels.ProductDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IZodiacProductService
    {
        Task<ServiceResponse<PaginationModel<ZodiacProductDTO>>> GetAllZodiacProduct(int page, int pageSize);
        Task<ServiceResponse<ZodiacProductDTO>> GetZodiacProductById(int id);
        Task<ServiceResponse<ZodiacProductDTO>> GetZodiacProductByProductId(int id);
        Task<ServiceResponse<int>> AddZodiacProduct(ZodiacProductDTO zodiacProduct);
        Task<ServiceResponse<string>> UpdateZodiacProduct(ZodiacProductDTO zodiacProduct);
        Task<ServiceResponse<string>> DeleteZodiacProduct(int id);
        Task<ServiceResponse<PaginationModel<ProductDTO>>> GetAllProductsByZodiacId(int zodiacId, int page, int pageSize, string search,Dictionary<string, string> filters, string sort);
    }
}
