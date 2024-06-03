using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.ProductDTO;
using AutoMapper;
using Domain.Entities;

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
                var newProduct = MapToEntity(product);
                newProduct.Id = 0; // Or whatever default value indicates it's a new entity

                await _productRepo.AddProduct(newProduct);
                // Assuming Id is the generated identifier
                serviceResponse.Data = newProduct.Id;

                // If the product was added successfully, set success to true
                serviceResponse.Success = true;
                serviceResponse.Message = "Product created successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to create product: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> UpdateProductAsync(ProductDTO product)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var validationContext = new ValidationContext(product);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(product, validationContext, validationResults, true))
                {
                    var errorMessages = validationResults.Select(r => r.ErrorMessage);
                    serviceResponse.Success = false;
                    serviceResponse.Message = string.Join("; ", errorMessages);
                    return serviceResponse;
                }

                var existingProduct = await _productRepo.GetProductById(product.Id);
                if (existingProduct == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Product not found";
                }
                else
                {
                    MapProductDTOToEntity(product, existingProduct);
                    await _productRepo.UpdateProduct(existingProduct);

                    // Set success message
                    serviceResponse.Data = "Product updated successfully";
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
            var productDTO = _mapper.Map<ProductDTO>(product);
            productDTO.ImageURLs = product.ProductImages?.Select(pi => pi.ImageUrl).ToList();
            return productDTO;
        }

        private IEnumerable<ProductDTO> MapToDTO(IEnumerable<Product> products)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        private Product MapToEntity(ProductDTO productDTO)
        {
            return _mapper.Map<Product>(productDTO);
        }

        private void MapProductDTOToEntity(ProductDTO productDTO, Product existingProduct)
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
