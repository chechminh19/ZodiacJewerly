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
        Task<ServiceResponse<PaginationModel<ProductImageDTO>>> GetAllImageInfors(int page, int pageSize, string search, string sort, int? productId);
        Task<ServiceResponse<ProductImageDTO>> GetImageInforById(int id);

    }
}
