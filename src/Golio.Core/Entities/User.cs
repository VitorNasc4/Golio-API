using System;
using System.Collections.Generic;

namespace Golio.Core.Entities
{
    public class User : BaseEntity
    {
        public User(string fullName, string email, string password, bool isAdmin)
        {
            FullName = fullName;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            Active = true;
            Password = password;
            IsAdmin = isAdmin;
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool IsAdmin { get; private set; } = false;
        public string Role { get; private set; } = "user";

        public void SetUserAdmin()
        {
            Role = "admin";
        }
    }
}
