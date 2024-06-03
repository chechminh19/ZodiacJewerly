using Application.IRepositories;
using Application.IService;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        private readonly AppDbContext _dbContext;
        public UserRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<bool> CheckEmailAddressExisted(string email) => await _dbContext.User.AnyAsync(u => u.Email == email);
        public async Task<bool> CheckPhoneNumberExited(string phonenumber) => await _dbContext.User.AnyAsync(x => x.TelephoneNumber == phonenumber);

        public async Task<User> GetUserByConfirmationToken(string token)
        {
            return await _dbContext.User.SingleOrDefaultAsync(
               u => u.ConfirmationToken == token
               );
        }

        public async Task<User> GetUserByEmailAddressAndPasswordHash(string email, string passwordHash)
        {
            var user = await _dbContext.User
                 .FirstOrDefaultAsync(record => record.Email == email && record.Password == passwordHash);
            if (user is null)
            {
                throw new Exception("Email & password is not correct");
            }
            return user;
        }
    }
}
