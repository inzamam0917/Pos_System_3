using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos_System_3.Model
{
    public enum UserRole
    {
        Admin,
        Cashier
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 100 characters.")]
        public string Password { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username length can't be more than 50.")]
        public string Username { get; set; }

        [Required]
        public UserRole UserRole { get; set; }
        public User() { }

        public User(string name, string email, string password, string username, int userId, UserRole userRole)
        {
            Name = name;
            Email = email;
            Password = password;
            Username = username;
            UserID = userId;
            UserRole = userRole;
        }
    }
}
