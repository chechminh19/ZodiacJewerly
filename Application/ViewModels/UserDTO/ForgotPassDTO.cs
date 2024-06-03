using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserDTO
{
    public class ForgotPassDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
