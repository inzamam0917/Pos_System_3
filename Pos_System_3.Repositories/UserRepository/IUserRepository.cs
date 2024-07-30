using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;

namespace Pos_System_3.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByEmailOrUsernameAsync(string emailOrUsername);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
