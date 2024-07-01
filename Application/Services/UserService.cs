using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
using Application.Ultilities;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsers(int page, int pageSize, string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsers();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users
                        .Where(c => c.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)||
                                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                users = sort.ToLower() switch
                {
                    "name" => users.OrderBy(c => c.FullName),
                    "email" => users.OrderBy(u => u.Email),
                    "role" => users.OrderBy(c => c.RoleName),
                    "status" => users.OrderBy(c => c.Status),
                    _ => users.OrderBy(c => c.Id).ToList()
                };
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);

                var paginationModel = await Pagination.GetPaginationIENUM(userDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve users: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByRole(string role, int page, int pageSize, string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersByRole(role);

                if (!string.IsNullOrEmpty(search))
                {
                    users = users
                        .Where(c => c.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)||
                                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                users = sort.ToLower() switch
                {
                    "name" => users.OrderBy(c => c.FullName),
                    "email" => users.OrderBy(u => u.Email),
                    "status" => users.OrderBy(c => c.Status),
                    _ => users.OrderBy(c => c.Id).ToList()
                };
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);

                var paginationModel = await Pagination.GetPaginationIENUM(userDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve users by role: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByStaff(int page, int pageSize, string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersStaff();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users
                        .Where(c => c.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)||
                                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                users = sort.ToLower() switch
                {
                    "name" => users.OrderBy(c => c.FullName),
                    "email" => users.OrderBy(u => u.Email),
                    "status" => users.OrderBy(c => c.Status),
                    _ => users.OrderBy(c => c.Id).ToList()
                };
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);

                var paginationModel = await Pagination.GetPaginationIENUM(userDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve staff users: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByAdmin(int page, int pageSize, string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersAdmin();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users
                        .Where(c => c.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)||
                                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                users = sort.ToLower() switch
                {
                    "name" => users.OrderBy(c => c.FullName),
                    "email" => users.OrderBy(u => u.Email),
                    "status" => users.OrderBy(c => c.Status),
                    _ => users.OrderBy(c => c.Id).ToList()
                };
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);

                var paginationModel = await Pagination.GetPaginationIENUM(userDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve admin users: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<PaginationModel<UserDTO>>> GetAllUsersByCustomer(int page, int pageSize, string search, string sort)
        {
            var response = new ServiceResponse<PaginationModel<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersCustomer();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users
                        .Where(c => c.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)||
                                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                users = sort.ToLower() switch
                {
                    "name" => users.OrderBy(c => c.FullName),
                    "email" => users.OrderBy(u => u.Email),
                    "status" => users.OrderBy(c => c.Status),
                    _ => users.OrderBy(c => c.Id).ToList()
                };
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);

                var paginationModel = await Pagination.GetPaginationIENUM(userDTOs, page, pageSize); // Adjust pageSize as needed

                response.Data = paginationModel;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Failed to retrieve customer users: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<UserDTO>> GetUserById(int id)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();

            try
            {
                var user = await _userRepo.GetUserById(id);
                if (user == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found";
                }
                else
                {
                    var userDTO = _mapper.Map<UserDTO>(user);
                    serviceResponse.Data = userDTO;
                    serviceResponse.Success = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> AddUser(UserDTO user)
        {
            var serviceResponse = new ServiceResponse<int>();

            try
            {
                var userEntity = _mapper.Map<User>(user);
                await _userRepo.AddUser(userEntity);

                serviceResponse.Data = userEntity.Id; // Assuming Id is set after insertion
                serviceResponse.Success = true;
                serviceResponse.Message = "User created successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to create user: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> UpdateUser(UserUpdateDTO userUpdate)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var userEntity = _mapper.Map<User>(userUpdate);
                await _userRepo.UpdateUser(userEntity);

                serviceResponse.Success = true;
                serviceResponse.Message = "User updated successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to update user: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteUser(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                await _userRepo.DeleteUser(id);

                serviceResponse.Success = true;
                serviceResponse.Message = "User deleted successfully";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to delete user: {ex.Message}";
            }

            return serviceResponse;
        }
    }
}
