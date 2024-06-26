﻿using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.ZodiacDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;
        private readonly IProductRepo _productRepo;
        private readonly IZodiacProductRepo _zodiPro;

        public OrderService(IOrderRepo orderRepo, IMapper mapper, IProductRepo product, IZodiacProductRepo zodiac)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productRepo = product;
            _zodiPro = zodiac;
        }


        public async Task<ServiceResponse<PaginationModel<OrderDTO>>> GetAllOrder(int page, int pageSize, string search,
            string filter, string sort)
        {
            var response = new ServiceResponse<PaginationModel<OrderDTO>>();

            try
            {
                var orders = await _orderRepo.GetAllOrders(); // Assuming this method supports async retrieval
                if (!string.IsNullOrEmpty(search))
                {
                    orders = orders.Where(o =>
                        o.User != null && o.User.FullName.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                if (byte.TryParse(filter, out byte status))
                {
                    orders = orders.Where(c => c.Status == status).ToList();
                }

                orders = sort.ToLower() switch
                {
                    "userid" => orders.OrderBy(o => o.UserId),
                    "payment" => orders.OrderBy(o => o.PaymentDate),
                    "status" => orders.OrderBy(o => o.Status),
                    _ => orders.OrderBy(o => o.Id) 
                };

                var orderDtOs = _mapper.Map<IEnumerable<OrderDTO>>(orders); // Map orders to OrderDTO

                // Apply pagination
                var paginationModel =
                    await Pagination.GetPaginationIENUM(orderDtOs, page, pageSize); // Adjusted pageSize as per original example

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve orders: {ex.Message}";
            }

            return response;
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

        public async Task<ServiceResponse<string>> AddProductToOrderAsync(int userId, int productId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var checkUserOrder = await _orderRepo.CheckUserWithOrder(userId);
                if (checkUserOrder == null)
                {
                    Order newOrder = new Order
                    {
                        UserId = userId,
                        Status = 1,
                        OrderDetails = new List<OrderDetails>()
                    };
                    OrderDetails newOrderDetail = new OrderDetails
                    {
                        ProductId = productId,
                        QuantityProduct = 1,
                        Price = _productRepo.GetProductPriceById(productId),
                    };
                    newOrder.OrderDetails.Add(newOrderDetail);
                    await _orderRepo.AddOrder(newOrder);
                    response.Success = true;
                    response.Message = "Add Product successfully when have no order already for user";
                }
                else
                {
                    var existingOrder = checkUserOrder.OrderDetails.FirstOrDefault(od => od.ProductId == productId);
                    if (existingOrder != null)
                    {
                        existingOrder.QuantityProduct += 1;
                        await _orderRepo.UpdateOrderDetail(existingOrder);
                        response.Success = true;
                        response.Message = "add duplicate success";
                    }
                    else
                    {
                        OrderDetails existingOrderDetail = new OrderDetails
                        {
                            OrderId = checkUserOrder.Id,
                            ProductId = productId,
                            QuantityProduct = 1,
                            Price = _productRepo.GetProductPriceById(productId),
                        };
                        await _orderRepo.AddOrderDetail(existingOrderDetail);
                        response.Success = true;
                        response.Message = "Add product successfully when already order for user";
                    }
                }
            }
            catch (DbException e)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { e.Message };
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { e.Message, e.StackTrace };
            }

            return response;
        }

        public async Task<ServiceResponse<CreateOrderDTO>> GetAllOrderCustomerCart(int userId)
        {
            var response = new ServiceResponse<CreateOrderDTO>();
            try
            {
                var listDetail = await _orderRepo.GetAllOrderCart(userId);
                if (listDetail == null)
                {
                    response.Success = true;
                    response.Message = "No order here";
                    response.Data = null;
                    response.PriceTotal = 0;
                    return response;
                }

                var productList = new List<ProductToCreateOrderDTO>();

                foreach (var detail in listDetail)
                {
                    var zodiacNames = _zodiPro.GetZodiacNameForProduct(detail.ProductId);
                    var productDto = new ProductToCreateOrderDTO
                    {
                        ProductId = detail.ProductId,
                        ZodiacName = zodiacNames,
                        NameProduct = detail.Product.NameProduct,
                        DescriptionProduct = detail.Product.DescriptionProduct,
                        Price = detail.Product.Price,
                        Quantity = detail.QuantityProduct,
                        NameMaterial = detail.Product.Material.NameMaterial,
                        NameCategory = detail.Product.Category.NameCategory,
                        NameGender = detail.Product.Gender.NameGender,
                        ImageUrl = detail.Product.ProductImages.FirstOrDefault()?.ImageUrl,
                    };
                    productList.Add(productDto);
                }

                double priceTotal = productList.Sum(productDto => productDto.Price * productDto.Quantity);
                response.Success = true;
                response.Data = new CreateOrderDTO { Product = productList, PriceTotal = priceTotal };
                return response;
            }
            catch (DbException e)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { e.Message };
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { e.Message, e.StackTrace };
            }

            return response;
        }
    }
}