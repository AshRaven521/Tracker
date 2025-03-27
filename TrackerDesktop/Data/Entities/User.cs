using System;
using System.Collections.Generic;

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
        public List<Step> Steps { get; set; } = new List<Step>();
        public List<Mode> Modes { get; set; } = new List<Mode>();

        public User()
        {

        }

        public User(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
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
