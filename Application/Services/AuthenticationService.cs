using Application.Commons;
using Application.IService;
using Application.ServiceResponse;
using Application.Utils;
using Application.ViewModels.UserDTO;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Data.Common;


namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _config;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationService(IMemoryCache _memory, IUnitOfWork unitOfWork, IMapper mapper,
            AppConfiguration configuration, ICurrentTime curTime)
        {
            _memoryCache = _memory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
            _currentTime = curTime;
        }

        public async Task<TokenResponse<string>> VerifyForgotPassCode(VerifyOTPResetDTO dto)
        {
            var response = new TokenResponse<string>();
            try
            {
                string key = $"{dto.Email}_OTP";
                if (_memoryCache.TryGetValue(key, out string? savedCode))
                {
                    if (savedCode == dto.CodeOTP)
                    {
                        response.Success = true;
                        response.Message = "OTP is valid.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Invalid OTP.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "OTP not found.";
                }

                return response;
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

        public async Task<TokenResponse<string>> ForgotPass(string email)
        {
            var response = new TokenResponse<string>();
            try
            {
                var existEmail = await _unitOfWork.UserRepository.CheckEmailAddressExisted(email);
                if (existEmail == false)
                {
                    response.Success = false;
                    response.Message = "Email not found";
                    return response;
                }
                var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username";
                    return response;
                }
                if (user.ConfirmationToken != null && !user.IsConfirmed)
                {
                    System.Console.WriteLine(user.ConfirmationToken + user.IsConfirmed);
                    response.Success = false;
                    response.Message = "Please confirm via link in your mail";
                    return response;
                }
                var tokenEmail = await GenerateRandomPasswordResetTokenByEmailAsync(email);
                var codeEmail = SendMail.GenerateRandomCodeWithExpiration(tokenEmail, 1);
                var codeEmailSent = await SendMail.SendResetPass(_memoryCache, email, codeEmail, false);
                if (!codeEmailSent)
                {
                    response.Success = false;
                    response.Message = "Error when sending email";
                    return response;
                }

                response.Success = true;
                response.Message = "An email with a code to reset your password has been sent.";
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
                response.Message = "An error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public Task<string> GenerateRandomPasswordResetTokenByEmailAsync(string email)
        {
            Random random = new Random();
            var token = "";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 8; i++)
            {
                token += chars[random.Next(chars.Length)];
            }

            return Task.FromResult(token);
        }

        public async Task<TokenResponse<string>> LoginAsync(LoginUserDTO userObject)
        {
            var response = new TokenResponse<string>();
            try
            {
                var passHash = HashPass.HashWithSHA256(userObject.Password);
                var userLogin =
                    await _unitOfWork.UserRepository.GetUserByEmailAddressAndPasswordHash(userObject.Email, passHash);
                if (userLogin == null)
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

                var auth = userLogin.RoleName;
                var userId = userLogin.Id;
                var token = userLogin.GenerateJsonWebToken(_config, _config.JWTSection.SecretKey, DateTime.Now);
                response.Success = true;
                response.Message = "Login successfully";
                response.DataToken = token;
                response.Role = auth;
                response.HintId = userId;
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
                userAccountRegister.Password = HashPass.HashWithSHA256(userObjectDTO.Password);
                //Create Token
                userAccountRegister.ConfirmationToken = Guid.NewGuid().ToString();

                userAccountRegister.Status = 1;
                userAccountRegister.RoleName = "Customer";
                await _unitOfWork.UserRepository.AddAsync(userAccountRegister);

                var confirmationLink =
                    $"https://zodiacjewerlyswd.azurewebsites.net/confirm?token={userAccountRegister.ConfirmationToken}";

                //SendMail
                var emailSend = await SendMail.SendConfirmationEmail(userObjectDTO.Email, confirmationLink);
                if (!emailSend)
                {
                    response.Success = false;
                    response.Message = "Error when send mail";
                    return response;
                }

                var accountRegistedDTO = _mapper.Map<RegisterDTO>(userAccountRegister);
                response.Success = true;
                response.Data = accountRegistedDTO;
                response.Message = "Register successfully.";
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
                response.ErrorMessages = new List<string> { e.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<ResetPassDTO>> ResetPass(ResetPassDTO dto)
        {
            var response = new ServiceResponse<ResetPassDTO>();
            try
            {
                var userAccount = await _unitOfWork.UserRepository.GetUserByEmailAsync(dto.Email);
                if (userAccount == null)
                {
                    response.Success = false;
                    response.Message = "Email not found";
                    return response;
                }

                if (dto.Password != dto.ConfirmPassword)
                {
                    response.Success = false;
                    response.Message = "Password and Confirm Password do not match.";
                    return response;
                }

                userAccount.Password = HashPass.HashWithSHA256(dto.Password);
                await _unitOfWork.UserRepository.Update(userAccount);


                var accountRegistedDTO = _mapper.Map<ResetPassDTO>(userAccount);
                response.Success = true;
                response.Message = "Password reset successfully.";
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
                response.ErrorMessages = new List<string> { e.Message };
            }

            return response;
        }

        public async Task<ServiceResponse<RegisterDTO>> CreateStaff(RegisterDTO userObject)
        {
            var response = new ServiceResponse<RegisterDTO>();
            try
            {
                var existEmail = await _unitOfWork.UserRepository.CheckEmailAddressExisted(userObject.Email);
                if (existEmail)
                {
                    response.Success = false;
                    response.Message = "Email is already existed";
                    return response;
                }

                var userAccountRegister = _mapper.Map<User>(userObject);
                userAccountRegister.Password = HashPass.HashWithSHA256(userObject.Password);
                //Create Token
                userAccountRegister.ConfirmationToken = Guid.NewGuid().ToString();

                userAccountRegister.Status = 1;
                userAccountRegister.RoleName = "Staff";

                await _unitOfWork.UserRepository.AddAsync(userAccountRegister);


                var confirmationLink =
                    $"https://zodiacjewerlyswd.azurewebsites.net/confirm?token={userAccountRegister.ConfirmationToken}";
                //var confirmationLink = $"https://your-api-domain/confirm?token={userAccountRegister.ConfirmationToken}&redirectUrl=https://your-frontend-domain/login";

                //SendMail
                var emailSend = await SendMail.SendConfirmationEmail(userObject.Email, confirmationLink);
                if (!emailSend)
                {
                    response.Success = false;
                    response.Message = "Error when send mail";
                    return response;
                }

                var accountRegistedDTO = _mapper.Map<RegisterDTO>(userAccountRegister);
                response.Success = true;
                response.Data = accountRegistedDTO;
                response.Message = "Register successfully.";
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
                response.ErrorMessages = new List<string> { e.Message };
            }

            return response;
        }
    }
}