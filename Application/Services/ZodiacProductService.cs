using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.ProductDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ZodiacProductService : IZodiacProductService
    {
        private readonly IZodiacProductRepo _zodiacProductRepo;
        private readonly IMapper _mapper;

        public ZodiacProductService(IZodiacProductRepo zodiacProductRepo, IMapper mapper)
        {
            _zodiacProductRepo = zodiacProductRepo ?? throw new ArgumentNullException(nameof(zodiacProductRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResponse<PaginationModel<ZodiacProductDTO>>> GetAllZodiacProduct(int page, int pageSize)
        {
            var response = new ServiceResponse<PaginationModel<ZodiacProductDTO>>();

            try
            {
                var zodiacProducts = await _zodiacProductRepo.GetAllZodiacProduct();
                var zodiacProductDTOs = _mapper.Map<IEnumerable<ZodiacProductDTO>>(zodiacProducts); // Map zodiac products to ZodiacProductDTO

                // Apply pagination
                var paginationModel = await Pagination.GetPaginationIENUM(zodiacProductDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve zodiac products: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<ZodiacProductDTO>> GetZodiacProductById(int id)
        {
            var serviceResponse = new ServiceResponse<ZodiacProductDTO>();

            try
            {
                var zodiacProduct = await _zodiacProductRepo.GetZodiacProductById(id);
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


        public async Task<ServiceResponse<ZodiacProductDTO>> GetZodiacProductByProductId(int id)
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

        public async Task<ServiceResponse<PaginationModel<ProductDTO>>> GetAllProductsByZodiacId(int zodiacId, int page, int pageSize, string search,Dictionary<string, string> filters, string sort)
        {
            {
                var serviceResponse = new ServiceResponse<PaginationModel<ProductDTO>>();

                try
                {
                    var products = await _zodiacProductRepo.GetAllProductsByZodiacId(zodiacId);

                    // Check if products were found
                    if (products == null || !products.Any())
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "No products found for the specified zodiac.";
                        return serviceResponse;
                    }

                    if (!string.IsNullOrEmpty(search))
                    {
                        products = products
                            .Where(c => c.NameProduct.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    foreach (var filter in filters)
                    {
                        switch (filter.Key.ToLower())
                        {
                            case "categoryid":
                                if (int.TryParse(filter.Value, out int categoryId))
                                {
                                    products = products.Where(p => p.CategoryId == categoryId);
                                }
                                break;
                            case "materialid":
                                if (int.TryParse(filter.Value, out int materialId))
                                {
                                    products = products.Where(p => p.MaterialId == materialId);
                                }
                                break;
                            case "genderid":
                                if (int.TryParse(filter.Value, out int genderId))
                                {
                                    products = products.Where(p => p.GenderId == genderId);
                                }
                                break;
                        }
                    }

                    products = sort.ToLower() switch
                    {
                        "name" => products.OrderBy(p => p.NameProduct),
                        "price" => products.OrderBy(p => p.Price),
                        "quantity" => products.OrderBy(p => p.Quantity),
                        "category" => products.OrderBy(p => p.CategoryId),
                        "material" => products.OrderBy(p => p.MaterialId),
                        "gender" => products.OrderBy(p => p.GenderId),
                        _ => products.OrderBy(p => p.Id) 
                    };
                    // Map products to ProductDTO and include ImageUrls
                    var productDTOs = products.Select(product => new ProductDTO
                    {
                        Id = product.Id,
                        NameProduct = product.NameProduct,
                        DescriptionProduct = product.DescriptionProduct,
                        Price = product.Price,
                        ZodiacId = zodiacId,
                        ImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList()
                    });

                    // Apply pagination
                    var paginationModel = await Pagination.GetPaginationIENUM(productDTOs, page, pageSize);

                    serviceResponse.Data = paginationModel;
                    serviceResponse.Success = true;
                }
                catch (Exception ex)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Failed to retrieve products for zodiac {zodiacId}: {ex.Message}";
                    // Log the exception for debugging
                }

                return serviceResponse;
            }
        }
    }
}







