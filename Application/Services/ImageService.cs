using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.ProductImageDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepo _imageRepo;
        private readonly IMapper _mapper;


        public ImageService(IImageRepo imageRepo, IMapper mapper)
        {
            _imageRepo = imageRepo ?? throw new ArgumentNullException(nameof(imageRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<ServiceResponse<IEnumerable<ProductImageDTO>>> GetAllOrderGetAllImageInfors()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<ProductImageDTO>>();

            try
            {
                var image = await _imageRepo.GetAllImageInfors();
                var imageDTOs = _mapper.Map<IEnumerable<ProductImageDTO>>(image);
                serviceResponse.Data = imageDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ProductImageDTO>> GetImageInforById(int id)
        {
            var serviceResponse = new ServiceResponse<ProductImageDTO>();

            try
            {
                var image = await _imageRepo.GetImageInforById(id);
                if (image == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Zodiac product not found";
                }
                else
                {
                    var imageDTO = _mapper.Map<ProductImageDTO>(image);
                    serviceResponse.Data = imageDTO;
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
