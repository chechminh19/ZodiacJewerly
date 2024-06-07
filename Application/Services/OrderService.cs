using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;


        public OrderService(IOrderRepo orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<ServiceResponse<IEnumerable<OrderDTO>>> GetAllOrder()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<OrderDTO>>();

            try
            {
                var order = await _orderRepo.GetAllOrders();
                var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(order);
                serviceResponse.Data = orderDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<OrderDTO>> GetOrderById(int id)
        {
            var serviceResponse = new ServiceResponse<OrderDTO>();

            try
            {
                var order = await _orderRepo.GetOrderById(id);
                if (order == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Zodiac product not found";
                }
                else
                {
                    var orderDTO = _mapper.Map<OrderDTO>(order);
                    serviceResponse.Data = orderDTO;
                    serviceResponse.Success = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }




        public async Task<ServiceResponse<int>> AddOrder(OrderDTO order)
        {
            var serviceResponse = new ServiceResponse<int>();

            try
            {
                // Map DTO to entity
                var OrderEntity = _mapper.Map<Order>(order);

                // Add zodiac product to repository
                await _orderRepo.AddOrder(OrderEntity);

                // Ensure the product ID is set after adding to the repository
                if (OrderEntity.Id == 0)
                {
                    throw new Exception("Failed to generate order ID.");
                }

                // Prepare success response
                serviceResponse.Data = OrderEntity.Id;
                serviceResponse.Success = true;
                serviceResponse.Message = "Order created successfully";
            }
            catch (Exception ex)
            {
                // Log the exception (assuming a logger is available)


                // Prepare failure response
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to create order: {ex.Message}";
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<string>> UpdateOrder(OrderDTO order)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var OrderEntity = _mapper.Map<Order>(order);
                await _orderRepo.UpdateOrder(OrderEntity);
                serviceResponse.Success = true;
                serviceResponse.Message = "order updated successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to update order: " + ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteOrder(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                await _orderRepo.DeleteOrder(id);
                serviceResponse.Success = true;
                serviceResponse.Message = "Order deleted successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to delete order: " + ex.Message;
            }

            return serviceResponse;
        }
    }
}
