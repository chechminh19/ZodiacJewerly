using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IProductService
    {
        //Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProductsAsync();
        Task<ServiceResponse<PaginationModel<ProductDTO>>> GetAllProductsAsync(int page);
        Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(int id);
        Task<ServiceResponse<int>> CreateProductAsync(CreateProductDTO cproduct, int zodiacId);
        Task<ServiceResponse<string>> UpdateProductAsync(CreateProductDTO cproduct, int zodiacId);
        Task<ServiceResponse<string>> DeleteProductAsync(int id);
        
    }
}
