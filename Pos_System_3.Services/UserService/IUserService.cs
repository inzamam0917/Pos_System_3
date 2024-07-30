using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;

namespace Pos_System_3.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(User user);
        Task<User> AuthenticateUserAsync(string emailOrUsername, string password);
        Task<bool> UpdateUserRoleAsync(string username, UserRole role);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
    }
}
