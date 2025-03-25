﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrackerDesktop.Data.Entities;

namespace TrackerDesktop.Data.Services
{
    public class TrackerDatabaseService : ITrackerDatabaseService
    {
        private readonly TrackerContext context;

        public TrackerDatabaseService(TrackerContext context)
        {
            this.context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken token)
        {
            var user = await context.Users.Where(x => x.Email == email).FirstOrDefaultAsync(token);

            return user;
        }

        public async Task<User?> GetLastLoggedInUserAsync(CancellationToken token)
        {
            var users = await context.Users.ToListAsync(token);

            if (!users.Any())
            {
                return null;
            }

            var loggedUsers = users.Where(u => u.IsLoggedIn == true);

            if (!loggedUsers.Any())
            {
                return null;
            }

            var lastLoggedUser = loggedUsers.Aggregate((s, a) => s.LastLoggedInDate < a.LastLoggedInDate ? s : a);

            return lastLoggedUser;
        }

        public async Task<User?> GetUserByNickNameAsync(string nickName, CancellationToken token)
        {
            var user = await context.Users.Where(x => x.UserName == nickName).FirstOrDefaultAsync(token);
            return user;
        }

        public async Task SaveUserAsync(User newUser, CancellationToken token)
        {
            var dbUser = await context.Users.FirstOrDefaultAsync(x => x.Id == newUser.Id, token);

            if (dbUser == null)
            {
                await context.Users.AddAsync(newUser, token);
                await context.SaveChangesAsync(token);
                return;
            }

            dbUser.UserName = newUser.UserName;
            dbUser.Email = newUser.Email;
            dbUser.Password = newUser.Password;
            dbUser.IsLoggedIn = newUser.IsLoggedIn;
            dbUser.LastLoggedInDate = newUser.LastLoggedInDate;

            await context.SaveChangesAsync(token);
        }

    }
}
