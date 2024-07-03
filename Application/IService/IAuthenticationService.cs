using Application.ServiceResponse;
using Application.ViewModels.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IAuthenticationService
    {
        public Task<ServiceResponse<RegisterDTO>> RegisterAsync(RegisterDTO userObject);
        public Task<ServiceResponse<RegisterDTO>> CreateStaff(RegisterDTO userObject);

        public Task<TokenResponse<string>> LoginAsync(LoginUserDTO userObject);
        public Task<TokenResponse<string>> ForgotPass(string email);
        public Task<string> GenerateRandomPasswordResetTokenByEmailAsync(string email);
        public Task<TokenResponse<string>> VerifyForgotPassCode(VerifyOTPResetDTO dto);
        public Task<ServiceResponse<ResetPassDTO>> ResetPass(ResetPassDTO dto);
    }
}