using Application.IRepositories;
using Application.IService;
using Application.ServiceResponse;
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

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsers();
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
                serviceResponse.Data = userDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByRole(string role)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersByRole(role);
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
                serviceResponse.Data = userDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByStaff()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersStaff();
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
                serviceResponse.Data = userDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByAdmin()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersAdmin();
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
                serviceResponse.Data = userDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> GetAllUsersByCustomer()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<UserDTO>>();

            try
            {
                var users = await _userRepo.GetAllUsersCustomer();
                var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
                serviceResponse.Data = userDTOs;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
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

        public async Task<ServiceResponse<string>> UpdateUser(UserDTO user)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var userEntity = _mapper.Map<User>(user);
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
