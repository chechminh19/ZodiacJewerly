using Application.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        private readonly AppDbContext _dbContext;

        public UserRepo(AppDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<bool> CheckEmailAddressExisted(string email) =>
            await _dbContext.User.AnyAsync(u => u.Email == email);

        public async Task<bool> CheckPhoneNumberExited(string phonenumber) =>
            await _dbContext.User.AnyAsync(x => x.TelephoneNumber == phonenumber);

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
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.User
                 .FirstOrDefaultAsync(record => record.Email == email);
            if (user is null)
            {
                throw new Exception("Email is not correct");
            }
            return user;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbContext.User.FindAsync(id);
        }

        public async Task<IEnumerable<User?>> GetAllUsers()
        {
            return _dbContext.User.ToList();
        }

        public async Task<int> CountUsersByRoleAsync(string roleName)
        {
            return await _dbContext.User
                .Where(u => u.RoleName == roleName)
                .CountAsync();
        }


        public async Task<IEnumerable<User?>> GetAllUsersAdmin()
        {
            return await _dbContext.User
                .Where(u => u.RoleName == "Admin")
                .ToListAsync();
        }

        public async Task<IEnumerable<User?>> GetAllUsersStaff()
        {
            return await _dbContext.User
                .Where(u => u.RoleName == "Staff")
                .ToListAsync();
        }

        public async Task<IEnumerable<User?>> GetAllUsersCustomer()
        {
            return await _dbContext.User
                .Where(u => u.RoleName == "Customer")
                .ToListAsync();
        }


        public async Task AddUser(User? user)
        {
            if (user != null) _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User updatedUser)
        {
            // Retrieve the existing user from the database
            var existingUser = await _dbContext.User.FindAsync(updatedUser.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Update the properties
            existingUser.FullName = updatedUser.FullName;
            existingUser.Email = updatedUser.Email;
            existingUser.Address = updatedUser.Address;
            existingUser.TelephoneNumber = updatedUser.TelephoneNumber;
            existingUser.Status = updatedUser.Status;
            existingUser.RoleName = updatedUser.RoleName;

            // Do not update the password to avoid setting it to null
            // existingUser.Password remains unchanged

            _dbContext.User.Update(existingUser);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteUser(int id)
        {
            var user = await _dbContext.User.FindAsync(id);
            if (user != null)
            {
                _dbContext.User.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        
    }
}