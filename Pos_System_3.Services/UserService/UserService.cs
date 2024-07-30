using System.Collections.Generic;
using System.Threading.Tasks;
using Pos_System_3.Model;
using Pos_System_3.Repositories.UserRepository;

namespace Pos_System_3.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task RegisterUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
                throw new System.Exception("User with this email already exists.");

            await _userRepository.AddUserAsync(user);
        }

        public async Task<User> AuthenticateUserAsync(string emailOrUsername, string password)
        {
            var user = await _userRepository.GetUserByEmailOrUsernameAsync(emailOrUsername);
            if (user == null || user.Password != password)
                return null;

            return user;
        }

        public async Task<bool> UpdateUserRoleAsync(string username, UserRole role)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return false;

            user.UserRole = role;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
    }
}
