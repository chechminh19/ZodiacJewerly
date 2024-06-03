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
        public Task<ServiceResponse<string>> LoginAsync(LoginUserDTO userObject);
        public Task<ServiceResponse<string>> ForgotPass(ForgotPassDTO userObject);
        //public Task<ServiceResponse<ResetPassDTO>> ResetPass(ResetPassDTO userObject);
    }
}
