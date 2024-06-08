using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.ProductDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ZodiacService : IZodiacProductService
    {
        private readonly IZodiacProductRepo _zodiacProductRepo;
        private readonly IMapper _mapper;

        public ZodiacService(IZodiacProductRepo zodiacProductRepo, IMapper mapper)
        {
            _zodiacProductRepo = zodiacProductRepo ?? throw new ArgumentNullException(nameof(zodiacProductRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


            try
            {
            var serviceResponse = new ServiceResponse<IEnumerable<ZodiacDTO>>();

            try
            {
                var zodiac = await _zodiacRepo.GetAllZodiacs();
                var zodiacDTOs = _mapper.Map<IEnumerable<ZodiacDTO>>(zodiac);
                serviceResponse.Data = zodiacDTOs;
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
            var serviceResponse = new ServiceResponse<ZodiacProductDTO>();

            try
            {
                var zodiacProduct = await _zodiacProductRepo.GetByProductId(id);
                if (zodiacProduct == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Zodiac product not found";
                }
                else
                {
                    var zodiacProductDTO = _mapper.Map<ZodiacProductDTO>(zodiacProduct);
                    serviceResponse.Data = zodiacProductDTO;
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

        public async Task<ServiceResponse<int>> AddZodiacProduct(ZodiacProductDTO zodiacProduct)
        {
            var serviceResponse = new ServiceResponse<int>();

            try
            {
                // Map DTO to entity
                var zodiacProductEntity = _mapper.Map<ZodiacProduct>(zodiacProduct);

                // Add zodiac product to repository
                await _zodiacProductRepo.AddZodiacProduct(zodiacProductEntity);

                // Ensure the product ID is set after adding to the repository
                if (zodiacProductEntity.Id == 0)
                {
                    throw new Exception("Failed to generate zodiac product ID.");
                }

                // Prepare success response
                serviceResponse.Data = zodiacProductEntity.Id;
                serviceResponse.Success = true;
                serviceResponse.Message = "Zodiac product created successfully";
            }
            catch (Exception ex)
            {
                // Log the exception (assuming a logger is available)
                

                // Prepare failure response
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to create zodiac product: {ex.Message}";
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<string>> UpdateZodiacProduct(ZodiacProductDTO zodiacProduct)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var zodiacProductEntity = _mapper.Map<ZodiacProduct>(zodiacProduct);
                await _zodiacProductRepo.UpdateZodiacProduct(zodiacProductEntity);
                serviceResponse.Success = true;
                serviceResponse.Message = "Zodiac product updated successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to update zodiac product: " + ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteZodiacProduct(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                await _zodiacProductRepo.DeleteZodiacProduct(id);
                serviceResponse.Success = true;
                serviceResponse.Message = "Zodiac product deleted successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to delete zodiac product: " + ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProductsByZodiacId(int zodiacId)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<ProductDTO>>();

            try
            {
                var products = await _zodiacProductRepo.GetAllProductsByZodiacId(zodiacId);
                if (products == null || !products.Any())
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No products found for the specified zodiac.";
                }
                else
                {
                    var productDTOs = new List<ProductDTO>();
                    foreach (var product in products)
                    {
                        var productDTO = _mapper.Map<ProductDTO>(product);
                        productDTO.ImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList();
                        productDTO.ZodiacId = zodiacId;
                        productDTOs.Add(productDTO);
                    }

                    serviceResponse.Data = productDTOs;
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
    }
}
