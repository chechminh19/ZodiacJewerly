using Application.Commons;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _user;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _config;
        //private IValidator<User> _validator;
        public AuthenticationService( UserManager<User> user,IUnitOfWork unitOfWork, IMapper mapper,AppConfiguration configuration, ICurrentTime curTime)
        {
            _user = user;   
            _unitOfWork = unitOfWork;
            _mapper = mapper;        
            _config = configuration;
            _currentTime = curTime;
        }

        public async Task<ServiceResponse<string>> ForgotPass(ForgotPassDTO userObject)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var checkEmailForm = Utils.IsValidEmailForm.IsValidEmail(userObject.Email);
                if (checkEmailForm == false)
                {
                    response.Success = false;
                    response.Message = "Invalid email form";
                    return response;
                }
                var existEmail = await _unitOfWork.UserRepository.CheckEmailAddressExisted(userObject.Email);
                if (!existEmail)
                {
                    response.Success = false;
                    response.Message = "Email is not found";
                    return response;
                }
                var code = await Utils.SendMail.SendResetPass(userObject.Email);
                if (!code)
                {
                    response.Success = false;
                    response.Message = "Error when send mail";
                    return response;
                }
                response.Success = true;
                response.Message = "Please get your code through your email";
            }
            catch (DbException e)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { e.Message };
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { e.Message, e.StackTrace };
            }
            return response;
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
                var token = userLogin.GenerateJsonWebToken(_config, _config.JWTSection.SecretKey, DateTime.Now);
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
                var checkEmailForm = Utils.IsValidEmailForm.IsValidEmail(userObjectDTO.Email);
                if (checkEmailForm == false)
                {                   
                    response.Success = false;
                    response.Message = "Invalid email form";
                    return response;
                }

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

                var confirmationLink = $"https://zodiacjewerly.azurewebsites.net/confirm?token={userAccountRegister.ConfirmationToken}";
                //SendMail
                var emailSend = await Utils.SendMail.SendConfirmationEmail(userObjectDTO.Email, confirmationLink);
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
        //public async Task<ServiceResponse<ResetPassDTO>> ResetPass(ResetPassDTO userObject)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
        //        if (user == null)
        //        {
        //            throw new Exception("User cannot be found");
        //        }
        //        if (resetPasswordDTO.Password != resetPasswordDTO.ConfirmPassword)
        //        {
        //            throw new Exception("Confirm password must match with password");
        //        }
        //        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Code, resetPasswordDTO.Password);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception("Password reset failed: " + result.Errors.FirstOrDefault()?.Description);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
