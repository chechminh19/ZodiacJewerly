﻿using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.SalesDTO;

namespace Application.IService
{
    public interface IOrderService
    {
        Task<ServiceResponse<PaginationModel<OrderDTO>>> GetAllOrder(int page, int pageSize, string search, string filter, string sort);
        Task<ServiceResponse<int>> AddOrder(OrderDTO order);
        Task<ServiceResponse<bool>> UpdateOrderQuantity(int orderId, int productId, int quantity);
        Task<ServiceResponse<bool>> RemoveProductFromCart(int orderId, int productId);
        Task<ServiceResponse<string>> AddProductToOrderAsync(int userId, int productId);
        Task<ServiceResponse<CreateOrderDTO>> GetAllOrderCustomerCart(int userId);
        Task<ServiceResponse<CreateOrderDTO>> GetAllOrderDetailById(int orderId);
        Task<ServiceResponse<string>> UpdateProductQuantitiesBasedOnCart(Order order);
        Task<ServiceResponse<string>> PaymentOrder(int orderId);
        Task<ServiceResponse<Dictionary<string, int>>> GetSalesByItemAsync();
        Task<ServiceResponse<List<SalesOverviewDTO>>> GetSalesOverviewAsync(int year);
    }
}
