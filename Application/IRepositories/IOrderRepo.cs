using Application.ViewModels.SalesDTO;
using Domain.Entities;

namespace Application.IRepositories
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<Order?> GetOrderById(int id);
        Task<Order> GetOrderByIdToPay(int orderId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task AddOrder(Order order);
        Task AddOrderDetail(OrderDetails orderDetail);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int id);
        Task<Order> CheckUserWithOrder(int userId);
        Task<List<OrderDetails>> GetAllOrderCart(int userId);
        Task<List<OrderDetails>> GetAllOrderDetailById(int orderId);
        Task UpdateOrderDetail(OrderDetails orderDetail);
        Task SaveChangesAsync();
        Task<int> GetProductSoldThisMonthAsync();
        Task<ICollection<OrderDetails>> GetOrderDetailsByOrderId(int orderId);

        Task<Dictionary<string, int>> GetSalesByItemAsync();
        Task<List<SalesOverviewDTO>> GetSalesOverviewAsync(int year);
    }
}