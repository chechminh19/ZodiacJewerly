using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.ZodiacDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ZodiacService : IZodiacService
    {
        private readonly IZodiacRepo _zodiacRepo;
        private readonly IMapper _mapper;


        public ZodiacService(IZodiacRepo zodiacRepo, IMapper mapper)
        {
            _zodiacRepo = zodiacRepo ?? throw new ArgumentNullException(nameof(zodiacRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<ServiceResponse<IEnumerable<ZodiacDTO>>> GetAllZodiacs()
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

        public async Task<ServiceResponse<ZodiacDTO>> GetZodiacById(int id)
        {
            var serviceResponse = new ServiceResponse<ZodiacDTO>();

            try
            {
                var zodiac = await _zodiacRepo.GetZodiacById(id);
                if (zodiac == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Zodiac product not found";
                }
                else
                {
                    var zodiacDTO = _mapper.Map<ZodiacDTO>(zodiac);
                    serviceResponse.Data = zodiacDTO;
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
