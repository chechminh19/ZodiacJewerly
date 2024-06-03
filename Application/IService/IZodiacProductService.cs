using Application.ServiceResponse;
using Application.ViewModels.ProductDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IZodiacProductService
    {
        Task<ServiceResponse<IEnumerable<ZodiacProductDTO>>> GetAllZodiacProduct();
        Task<ServiceResponse<ZodiacProductDTO>> GetZodiacProductById(int id);
        Task<ServiceResponse<ZodiacProductDTO>> GetZodiacProductByProductId(int id);
        Task<ServiceResponse<int>> AddZodiacProduct(ZodiacProductDTO zodiacProduct);
        Task<ServiceResponse<string>> UpdateZodiacProduct(ZodiacProductDTO zodiacProduct);
        Task<ServiceResponse<string>> DeleteZodiacProduct(int id);
        Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProductsByZodiacId(int zodiacId);
    }
}
