
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IZodiacProductRepo _zodiacProductRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepo productRepo, IZodiacProductRepo zodiacProductRepo, IMapper mapper)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _zodiacProductRepo = zodiacProductRepo ?? throw new ArgumentNullException(nameof(zodiacProductRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProductsAsync()
        {
            var response = new ServiceResponse<IEnumerable<ProductDTO>>();

            try
            {
                var products = await _productRepo.GetAllProduct();
                var productDTOs = MapToDTO(products);
                response.Data = productDTOs;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve products: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(int id)
        {
            var response = new ServiceResponse<ProductDTO>();

            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Product not found";
                }
                else
                {
                    var productDTO = MapToDTO(product);
                    response.Data = productDTO;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve product: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> CreateProductAsync(CreateProductDTO product, int zodiacId)
        {
            var response = new ServiceResponse<int>();

            try
            {
                var newProduct = MapToEntityCreate(product);
                newProduct.Id = 0;

                await _productRepo.AddProduct(newProduct);
                response.Data = newProduct.Id;
                response.Success = true;
                response.Message = "Product created successfully";

                // Create the ZodiacProduct entity
                var newZodiacProduct = new ZodiacProduct
                {
                    ProductId = newProduct.Id,
                    ZodiacId = zodiacId
                };

                // Add the ZodiacProduct entity to the repository
                await _zodiacProductRepo.AddZodiacProduct(newZodiacProduct);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<string>> UpdateProductAsync(CreateProductDTO product, int zodiacId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                // Validate the product DTO
                var validationContext = new ValidationContext(product);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(product, validationContext, validationResults, true))
                {
                    var errorMessages = validationResults.Select(r => r.ErrorMessage);
                    response.Success = false;
                    response.Message = string.Join("; ", errorMessages);
                    return response;
                }

                // Retrieve the existing product from the repository
                var existingProduct = await _productRepo.GetProductById(product.Id);
                if (existingProduct == null)
                {
                    response.Success = false;
                    response.Message = "Product not found";
                    return response;
                }

                // Map updated values from DTO to the existing entity
                MapCreateProductDTOToEntity(product, existingProduct);

                // Update the product in the repository
                await _productRepo.UpdateProduct(existingProduct);

                // Retrieve the existing ZodiacProduct entity
                var existingZodiacProduct = await _zodiacProductRepo.GetByProductId(product.Id);
                if (existingZodiacProduct == null)
                {
                    response.Success = false;
                    response.Message = "Associated ZodiacProduct not found";
                    return response;
                }

                // Update the ZodiacProduct entity with the new zodiacId
                existingZodiacProduct.ZodiacId = zodiacId;
                await _zodiacProductRepo.UpdateZodiacProduct(existingZodiacProduct);

                response.Data = "Product and associated ZodiacProduct updated successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                // Log the exception
                

                response.Success = false;
                response.Message = $"Failed to update product: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<string>> DeleteProductAsync(int id)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var existingProduct = await _productRepo.GetProductById(id);
                if (existingProduct == null)
                {
                    response.Success = false;
                    response.Message = "Product not found";
                }
                else
                {
                    await _productRepo.DeleteProduct(id);
                    response.Data = "Product deleted successfully";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to delete product: {ex.Message}";
            }

            return response;
        }

        private ProductDTO MapToDTO(Product product)
        {
            var productDTO = _mapper.Map<ProductDTO>(product);
            productDTO.ImageUrls = product.ProductImages?.Select(pi => pi.ImageUrl).ToList();
            productDTO.ZodiacId = (int)(product.ProductZodiacs?.Select(pi => pi.ZodiacId).FirstOrDefault());



            return productDTO;
        }
        private CreateProductDTO MapToDTOCreate(Product product)
        {
            var productDTO = _mapper.Map<CreateProductDTO>(product);

            return productDTO;
        }
        private IEnumerable<ProductDTO> MapToDTO(IEnumerable<Product> products)
        {
            return products.Select(MapToDTO);
        }

        private Product MapToEntity(ProductDTO productDTO)
        {
            return _mapper.Map<Product>(productDTO);
        }

        private Product MapToEntityCreate(CreateProductDTO CreateProductDTO)
        {
            return _mapper.Map<Product>(CreateProductDTO);
        }

        private void MapCreateProductDTOToEntity(CreateProductDTO productDTO, Product existingProduct)
        {
            existingProduct.NameProduct = productDTO.NameProduct;
            existingProduct.DescriptionProduct = productDTO.DescriptionProduct;
            existingProduct.Price = productDTO.Price;
            existingProduct.Quantity = productDTO.Quantity;
            existingProduct.CategoryId = productDTO.CategoryId;
            existingProduct.MaterialId = productDTO.MaterialId;
            existingProduct.GenderId = productDTO.GenderId;
        }
    }
}

