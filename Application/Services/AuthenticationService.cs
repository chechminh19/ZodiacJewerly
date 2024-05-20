using Application.Commons;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _config;
        //private IValidator<User> _validator;
        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper,AppConfiguration configuration, ICurrentTime curTime)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;        
            _config = configuration;
            _currentTime = curTime;
        }

        public Task<ServiceResponse<string>> ForgotPass(LoginUserDTO userObj)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginUserDTO userObject)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var passHash = Utils.HashPass.HashWithSHA256(userObject.Password);
                var userLogin = await _unitOfWork.UserRepository.GetUserByEmailAddressAndPasswordHash(userObject.Email, passHash);
                if(userLogin == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                if (userLogin.ConfirmationToken != null && !userLogin.IsConfirmed)
                {
                    System.Console.WriteLine(userLogin.ConfirmationToken + userLogin.IsConfirmed);
                    response.Success = false;
                    response.Message = "Please confirm via link in your mail";
                    return response;
                }
                var token = userLogin.GenerateJsonWebToken(_config, _config.JWTSection.SecretKey, _currentTime.GetCurrentTime());
                response.Success = true;
                response.Message = "Login successfully";
                response.Data = token;  
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;

        }

        public async Task<ServiceResponse<RegisterDTO>> RegisterAsync(RegisterDTO userObjectDTO)
        {
            var response = new ServiceResponse<RegisterDTO>();
            try
            {
                var existEmail = await _unitOfWork.UserRepository.CheckEmailAddressExisted(userObjectDTO.Email);
                if (existEmail)
                {
                    response.Success = false;
                    response.Message = "Email is already existed";
                    return response;
                }
                var userAccountRegister = _mapper.Map<User>(userObjectDTO);
                userAccountRegister.Password = Utils.HashPass.HashWithSHA256(userObjectDTO.Password);
                //Create Token
                userAccountRegister.ConfirmationToken = Guid.NewGuid().ToString();

                userAccountRegister.Status = 1;
                userAccountRegister.RoleName = "Customer";
                await _unitOfWork.UserRepository.AddAsync(userAccountRegister);

                var confirmLink = $"https://localhost:7187/swagger/confirm?token={userAccountRegister.ConfirmationToken}";
                //SendMail
                var emailSend = await Utils.SendMail.SendConfirmationEmail(userObjectDTO.Email, confirmLink);
                if (!emailSend)
                {
                    response.Success = false;
                    response.Message = "Error when send mail";
                    return response;
                }
                else 
                {
                    var success = await _unitOfWork.SaveChangeAsync() > 0;
                    if (success)
                    {
                        var accountRegistedDTO = _mapper.Map<RegisterDTO>(userAccountRegister);
                        response.Success = true;
                        response.Data = accountRegistedDTO;
                        response.Message = "Register successfully.";                       
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error when saving ur account";
                    }
                }
            }
            catch(DbException e)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { e.Message };
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { e.Message, e.StackTrace };
            }
            return response;
        }
    }
}
