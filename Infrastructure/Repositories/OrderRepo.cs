using Application.Enums;
using Application.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepo : GenericRepo<Order>, IOrderRepo
    {
        private readonly AppDbContext _dbContext;

        public OrderRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _dbContext.Order.FindAsync(id);
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbContext.Order
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var orders = await _dbContext.Order.Include(o => o.User).Include(o => o.OrderDetails).ToListAsync();
            return  orders;
        }

        public async Task AddOrder(Order order)
        {
            _dbContext.Order.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _dbContext.Order.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _dbContext.Order.FindAsync(id);
            if (order != null)
            {
                _dbContext.Order.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Order> CheckUserWithOrder(int userId)
        {
            var order = await _dbContext.Order.Include(p => p.OrderDetails).FirstOrDefaultAsync(p=>p.UserId == userId);
            return order;
        }

        public async Task AddOrderDetail(OrderDetails orderDetail)
        {
            _dbContext.OrderDetail.Add(orderDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderDetails>> GetAllOrderCart(int userId)
        {            
            return  _dbContext.Order
                .Where(o => o.UserId == userId && o.Status == (byte)OrderCart.Process)
                    .SelectMany(o => o.OrderDetails)
                    .Include(od => od.Product)
                        .ThenInclude(p => p.Category)
                    .Include(od => od.Product)
                        .ThenInclude(p => p.Material)
                    .Include(od => od.Product)
                        .ThenInclude(p => p.Gender)
                    .Include(od => od.Product)
                        .ThenInclude(p => p.ProductImages)
                    .Include(od => od.Product)
                        .ThenInclude(p => p.ProductZodiacs)
                            .ThenInclude(pz => pz.Zodiac)
                    .ToList();      
        }

        public async Task<List<OrderDetails>> GetAllOrderDetailById(int orderId)
        {
            return await _dbContext.Order
                .Where(o => o.Id == orderId)
                .SelectMany(o => o.OrderDetails)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Category)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Material)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Gender)
                .Include(od => od.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(od => od.Product)
                    .ThenInclude(p => p.ProductZodiacs)
                        .ThenInclude(pz => pz.Zodiac)
                .ToListAsync();
        }

        public async Task UpdateOrderDetail(OrderDetails orderDetail)
        {
            _dbContext.OrderDetail.Update(orderDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<OrderDetails>> GetAllOrderCartToPaid(long orderID)
        {
            return await _dbContext.Order
               .Where(o => o.Id == orderID && o.Status == (byte)OrderCart.Process)
                   .SelectMany(o => o.OrderDetails)
                   .Include(od => od.Product)
                       .ThenInclude(p => p.Category)
                   .Include(od => od.Product)
                       .ThenInclude(p => p.Material)
                   .Include(od => od.Product)
                       .ThenInclude(p => p.Gender)
                   .Include(od => od.Product)
                       .ThenInclude(p => p.ProductImages)
                   .Include(od => od.Product)
                       .ThenInclude(p => p.ProductZodiacs)
                           .ThenInclude(pz => pz.Zodiac)
                   .ToListAsync();
        }
    }
}
