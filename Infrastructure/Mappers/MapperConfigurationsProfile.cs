using Application.ViewModels.ProductDTO;
using Application.ViewModels.UserDTO;
using Application.ViewModels.ZodiacDTO;
using Application.ViewModels.OrderDTO;
using AutoMapper;
using Domain.Entities;
using Application.ViewModels.CollectionsDTO;
using Application.ViewModels.MaterialDTO;
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
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserCountDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Zodiac, ProductDTO>().ReverseMap();
            CreateMap<ZodiacProduct, ProductDTO>().ReverseMap();
            CreateMap<ZodiacProduct, ZodiacProductDTO>().ReverseMap();
            CreateMap<Category, CategoryReqDTO>().ReverseMap();
            CreateMap<Category, CategoryResDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.FullName))
                .ReverseMap();
            CreateMap<Zodiac, ZodiacDTO>().ReverseMap();
            CreateMap<Zodiac, ZodiacUpdateDTO>().ReverseMap();
            CreateMap<ProductImage, ProductImageDTO>().ReverseMap();
            CreateMap<Collections, CollectionsResDTO>().ReverseMap();
            CreateMap<Collections, CollectionsReqDTO>().ReverseMap();
            CreateMap<Material, MaterialResDTO>().ReverseMap();
            CreateMap<Material, MaterialReqDTO>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsResDTO>().ReverseMap();




        }
    }
}

