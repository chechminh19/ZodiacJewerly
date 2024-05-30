using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetAllProduct();
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
