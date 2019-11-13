using System;
using System.Collections.Generic;
using System.Text;

namespace User.Domain.Entity
{
    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }
        private string Password { get; set; }

        public bool PasswordIsValid(string password)
        {
            if (password == Password)
                return true;
            return false;
        }

        public void SetPassword(string password)
        {
            //codingencoding
            Password = password;
        }

    }
}
