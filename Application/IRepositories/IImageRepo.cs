using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ViewModels.OrderDTO;
using Domain.Entities;

namespace Application.IRepositories
{
    public interface IImageRepo : IGenericRepo<ProductImage>
    {
        Task<ProductImage> GetImageInforById(int id);
        Task<IEnumerable<ProductImage>> GetAllImageInfors();

       
    }
}
