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
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsers(int page);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByRole(string role,int page);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByStaff(int page);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByAdmin(int page);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByCustomer(int page);
        Task<ServiceResponse<UserDTO>> GetUserById(int id);
        Task<ServiceResponse<int>> AddUser(UserDTO user);
        Task<ServiceResponse<string>> UpdateUser(UserDTO user);
        Task<ServiceResponse<string>> DeleteUser(int id);
     
    }
}
