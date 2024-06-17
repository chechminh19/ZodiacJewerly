using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ViewModels.OrderDTO;
using Domain.Entities;

namespace Application.IRepositories
{
    public interface IZodiacRepo : IGenericRepo<Zodiac>
    {
        Task<Zodiac?> GetZodiacById(int id);
        Task<IEnumerable<Zodiac>> GetAllZodiacs();

        Task UpdateZodiac(Zodiac zodiac);


    }
}
