using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User> GetUserByEmailAddressAndPasswordHash(string email, string passwordHash);
        Task<bool> CheckEmailAddressExisted(string emailaddress);
        Task<bool> CheckPhoneNumberExited(string phonenumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByConfirmationToken(string token);
    }
}
