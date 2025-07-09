using UserMicroservice.Models;

namespace UserMicroservice.Services.DAO
{
    public interface IUserRepo
    {
        Task<User> GetUserById(int userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(int userId, User user);
        Task<bool> DeleteUser(int userId);
        Task<User> GetUserByUsername(string username);
    }
}
