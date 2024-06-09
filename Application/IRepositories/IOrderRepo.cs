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
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);
       
    }
}
