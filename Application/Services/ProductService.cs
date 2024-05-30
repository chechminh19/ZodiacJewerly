using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepo productRepo, IMapper mapper)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProductsAsync()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<ProductDTO>>();

            try
            {
                var products = await _productRepo.GetAllProduct();
                var productDTOs = MapToDTO(products);
                serviceResponse.Data = productDTOs;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<ProductDTO>();

            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Product not found";
                }
                else
                {
                    var productDTO = MapToDTO(product);
                    serviceResponse.Data = productDTO;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> CreateProductAsync(ProductDTO product)
        {
            var serviceResponse = new ServiceResponse<int>();

            try
            {
                // Map ProductDTO to Product entity if needed
                var newProduct = MapToEntity(product);
                await _productRepo.AddProduct(newProduct);
                // Assuming Id is the generated identifier
                serviceResponse.Data = newProduct.Id;

                // If the product was added successfully, set success to true
                serviceResponse.Success = true;
                serviceResponse.Message = "Product created successfully";
            }
            catch (Exception ex)
            {
                // If an exception occurred, set success to false and provide an error message
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to create product: " + ex.Message;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<string>> UpdateProductAsync(ProductDTO product)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                // Map ProductDTO to Product entity if needed
                var existingProduct = await _productRepo.GetProductById(product.Id);
                if (existingProduct == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Product not found";
                }
                else
                {
                    // Update existing product entity with new data
                    // Assuming there's some mapping or updating logic here
                    await _productRepo.UpdateProduct(existingProduct);
                    serviceResponse.Data = "Product updated successfully";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteProductAsync(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var existingProduct = await _productRepo.GetProductById(id);
                if (existingProduct == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Product not found";
                }
                else
                {
                    await _productRepo.DeleteProduct(id);
                    serviceResponse.Data = "Product deleted successfully";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        private ProductDTO MapToDTO(Product product)
        {
            return _mapper.Map<ProductDTO>(product);
        }

        private IEnumerable<ProductDTO> MapToDTO(IEnumerable<Product> products)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        private Product MapToEntity(ProductDTO productDTO)
        {
            return _mapper.Map<Product>(productDTO);
        }
    }
}
