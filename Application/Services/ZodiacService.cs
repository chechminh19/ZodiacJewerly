using Application.IRepositories;
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
using System.ComponentModel.DataAnnotations;
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


        public async Task<ServiceResponse<PaginationModel<ZodiacDTO>>> GetAllZodiacs(int page, int pageSize,
            string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<ZodiacDTO>>();

            try
            {
                var zodiacs = await _zodiacRepo.GetAllZodiacs(); 
                if (!string.IsNullOrEmpty(search))
                {
                    zodiacs = zodiacs
                        .Where(c => c.NameZodiac.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                zodiacs = sort.ToLower() switch
                {
                    "name" => zodiacs.OrderBy(p => p.NameZodiac),
                    _ => zodiacs.OrderBy(p => p.Id) 
                };
                var zodiacDTOs = _mapper.Map<IEnumerable<ZodiacDTO>>(zodiacs); // Map zodiacs to ZodiacDTO

                // Apply pagination
                var paginationModel = await Pagination.GetPaginationIENUM(zodiacDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve zodiacs: {ex.Message}";
            }

            return response;
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


        public async Task<ServiceResponse<string>> UpdateZodiac(ZodiacUpdateDTO zodiacUpdateDTO)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                // Validate the input
                if (zodiacUpdateDTO == null || string.IsNullOrEmpty(zodiacUpdateDTO.DesZodiac))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid Zodiac data";
                    return serviceResponse;
                }

                // Retrieve the existing zodiac entity
                var existingZodiac = await _zodiacRepo.GetZodiacById(zodiacUpdateDTO.Id);
                if (existingZodiac == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Zodiac not found";
                    return serviceResponse;
                }

                // Map the updated data to the existing entity
                _mapper.Map(zodiacUpdateDTO, existingZodiac);

                // Save the updated entity to the database
                await _zodiacRepo.UpdateZodiac(existingZodiac);

                serviceResponse.Success = true;
                serviceResponse.Message = "Zodiac updated successfully";
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to update zodiac: " + ex.Message;
            }

            return serviceResponse;
        }

    }
}
