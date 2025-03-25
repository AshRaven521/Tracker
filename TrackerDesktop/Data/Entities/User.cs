using System;

namespace TrackerDesktop.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsLoggedIn { get; set; } = false;

        public DateTime LastLoggedInDate { get; set; }

        public User()
        {

        }

        public User(string userName, string password, string email, bool isLoggedIn, DateTime loggedInDate)
        {
            UserName = userName;
            Password = password;
            Email = email;
            IsLoggedIn = isLoggedIn;
            LastLoggedInDate = loggedInDate;
        }
    }
}
