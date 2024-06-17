
using Application.IService;
using Application.Services;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.UserDTO;
using Application.ViewModels.ZodiacDTO;
using Application.ViewModels.OrderDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.ProductImageDTO;

namespace Infrastructure.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<ResetPassDTO, User>()
                 .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<User, ResetPassDTO>();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<ZodiacProduct, ZodiacProductDTO>().ReverseMap();
            CreateMap<Category, CategoryReqDTO>().ReverseMap();
            CreateMap<Category, CategoryResDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Zodiac, ZodiacDTO>().ReverseMap();
            CreateMap<Zodiac, ZodiacUpdateDTO>().ReverseMap();
            CreateMap<ProductImage, ProductImageDTO>().ReverseMap();
            //CreateMap<IAuthenticationService, AuthenticationService>();
        }
    }
}

