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
    public interface IOrderService
    {
        Task<ServiceResponse<PaginationModel<OrderDTO>>> GetAllOrder(int page);
        Task<ServiceResponse<OrderDTO>> GetOrderById(int id);
        Task<ServiceResponse<int>> AddOrder(OrderDTO order);
        Task<ServiceResponse<string>> UpdateOrder(OrderDTO order);
        Task<ServiceResponse<string>> DeleteOrder(int id);
        Task<ServiceResponse<string>> AddProductToOrderAsync(int userId, int productId);
        Task<ServiceResponse<CreateOrderDTO>> GetAllOrderCustomerCart(int userId);
    }
}
