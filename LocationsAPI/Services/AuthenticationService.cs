using LocationsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web.Helpers;

namespace LocationsAPI.Services
{
    public interface IAuthenticationService
    {
        public Task<User?> GetUserAsync(string username, string password);
        public Task<int> CreateUserAsync(string username, string password);
    }


    public class AuthenticationService : IAuthenticationService
    {
        private readonly LocationContext _dbContext;

        public AuthenticationService(LocationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }

            if (!Crypto.VerifyHashedPassword(user.Password, password))
            {
                return null;
            }

            return user;
        }

        public async Task<int> CreateUserAsync(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    // If username or password are null/empty, returns 0
                    return 0;
                }

                await _dbContext.Users.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Password = Crypto.HashPassword(password),
                });

                var result = await _dbContext.SaveChangesAsync();
                // If operation was successful, returns any number > 0;
                return result;
            }
            else
            {
                // If the user already exists, returns -1
                return -1;
            }
        }
    }
}
