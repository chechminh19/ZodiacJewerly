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
        Task<User> GetUserByEmail(string email);

        Task<bool> CheckEmailAddressExisted(string emailaddress);
        Task<bool> CheckPhoneNumberExited(string phonenumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByConfirmationToken(string token);
        Task<User> GetUserById(int id);
        Task<IEnumerable<User?>> GetAllUsers();
        Task<int> CountUsersByRoleAsync(string role);
        Task<IEnumerable<User?>> GetAllUsersAdmin();
        Task<IEnumerable<User?>> GetAllUsersStaff();
        Task<IEnumerable<User?>> GetAllUsersCustomer();
        Task AddUser(User? user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
