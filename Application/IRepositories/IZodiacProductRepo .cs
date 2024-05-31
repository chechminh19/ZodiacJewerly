using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IZodiacProductRepo : IGenericRepo<ZodiacProduct>
    {
        Task<ZodiacProduct> GetZodiacProductById(int id);
        Task<IEnumerable<ZodiacProduct>> GetAllZodiacProduct();
        Task AddZodiacProduct(ZodiacProduct zproduct);
        Task UpdateZodiacProduct(ZodiacProduct zproduct);
        Task DeleteZodiacProduct(int id);

        Task<IEnumerable<Product>> GetAllProductsByZodiacId(int zodiacId);


    }
}
