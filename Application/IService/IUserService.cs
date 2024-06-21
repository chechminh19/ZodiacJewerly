using Application.ServiceResponse;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IUserService
    {
        Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsers();
        Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByRole(string role);
        Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByStaff();
        Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByAdmin();
        Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByCustomer();
        Task<ServiceResponse<UserDTO>> GetUserById(int id);
        Task<ServiceResponse<int>> AddUser(UserDTO user);
        Task<ServiceResponse<string>> UpdateUser(UserDTO user);
        Task<ServiceResponse<string>> DeleteUser(int id);
     
    }
}
