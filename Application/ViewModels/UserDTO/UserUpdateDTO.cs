using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserDTO
{
    public class UserUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? TelephoneNumber { get; set; }
        public byte Status { get; set; }
        public string? RoleName { get; set; }
    }
}
