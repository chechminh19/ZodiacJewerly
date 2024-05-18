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

        public Task<bool> CheckEmailAddressExisted(string email) => _dbContext.Users.AnyAsync(u => u.Email == email);
        public Task<bool> CheckPhoneNumberExited(string phonenumber) => _dbContext.Users.AnyAsync(x => x.TelephoneNumber == phonenumber);

        public Task<User> GetUserByConfirmationToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmailAddressAndPasswordHash(string username, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
