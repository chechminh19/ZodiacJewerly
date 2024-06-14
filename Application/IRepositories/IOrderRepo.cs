using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ViewModels.OrderDTO;
using Domain.Entities;

namespace Application.IRepositories
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetAllOrders();
        Task AddOrder(Order order);
        Task AddOrderDetail(OrderDetails orderDetail);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);
        Task<Order> CheckUserWithOrder(int userId);
        Task<List<OrderDetails>> GetAllOrderCart(int userId);
        Task UpdateOrderDetail(OrderDetails orderDetail);
    }
}
