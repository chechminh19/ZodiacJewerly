﻿using Application.ServiceResponse;
using Application.ViewModels.ProductImageDTO;
using Application.ViewModels.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IImageService
    {
        Task<ServiceResponse<IEnumerable<ProductImageDTO>>> GetAllOrderGetAllImageInfors();
        Task<ServiceResponse<ProductImageDTO>> GetImageInforById(int id);

    }
}
