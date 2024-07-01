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
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsers(int page, int pageSize, string search, string sort);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByRole(string role,int page, int pageSize, string search, string sort);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByStaff(int page, int pageSize, string search, string sort);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByAdmin(int page, int pageSize, string search, string sort);
        Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByCustomer(int page, int pageSize, string search, string sort);
        Task<ServiceResponse<UserDTO>> GetUserById(int id);
        Task<ServiceResponse<int>> AddUser(UserDTO user);
        Task<ServiceResponse<string>> UpdateUser(UserUpdateDTO userUpdate);
        Task<ServiceResponse<string>> DeleteUser(int id);
     
    }
}
