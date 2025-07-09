using Microsoft.AspNetCore.Identity;
using UserMicroservice.Data;
using UserMicroservice.Models;
using UserMicroservice.Models.DTO;
using UserMicroservice.Services.DAO;

namespace UserMicroservice.Services.Repository
{
    public class AuthRepository : IAuthRepo
    {

        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public AuthResponseModel? Login(LoginModel loginModel)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == loginModel.UserName);
            if (user == null) return null;

            if (user.UserName != loginModel.UserName) return null;

            var validPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, loginModel.Password);
            if (validPassword != PasswordVerificationResult.Success) return null;

            Token t = new Token(_configuration);
            string token = t.GenerateJwtToken(user);

            return new AuthResponseModel
            {
                Id = user.UserId,
                UserName = user.UserName,
                Token = token,
                Role = user.Role.ToString()
            };
        }
    }
}
