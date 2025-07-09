using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserMicroservice.Data;
using UserMicroservice.Models;
using UserMicroservice.Services.DAO;

namespace UserMicroservice.Services.Repository
{
    public class UserRepository : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserRepository(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> CreateUser(User user)
        {
            var result = _context.Users.FirstOrDefault(c => c.UserId == user.UserId);
            if (result != null) return false;

            user.Password = _passwordHasher.HashPassword(user, user.Password);

            // Set CreatedAt to Indian Standard Time (IST)
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            user.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indiaTimeZone);
            user.UpdatedAt = user.CreatedAt; // Initially, UpdatedAt is the same as CreatedAt

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var result = _context.Users.FirstOrDefault(c => c.UserId == userId);
            if (result == null)
            {
                return false;
            }
            _context.Users.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return _context.Users;
        }

        public async Task<User> GetUserById(int userId)
        {
            var result = await _context.Users.FirstOrDefaultAsync(c => c.UserId == userId);
            return result;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var result = await _context.Users.FirstOrDefaultAsync(c => c.UserName.ToLower().Contains(username.ToLower()));
            return result;
        }

        public async Task<bool> UpdateUser(int userId, User user)
        {
            var existingUser = _context.Users.FirstOrDefault(c => c.UserId == userId);
            if (existingUser == null) return false;


            existingUser.UserName = user.UserName;
            existingUser.Password = _passwordHasher.HashPassword(existingUser, user.Password);

            // Set UpdatedAt to Indian Standard Time (IST)
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            existingUser.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indiaTimeZone);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
