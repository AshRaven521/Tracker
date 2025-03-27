using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                return new User(false);
            }

            var loggedUsers = users.Where(u => u.IsLoggedIn == true);

            if (!loggedUsers.Any())
            {
                return new User(false);
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
            dbUser.Steps = newUser.Steps;
            dbUser.Modes = newUser.Modes;
            dbUser.Email = newUser.Email;
            dbUser.Password = newUser.Password;
            dbUser.IsLoggedIn = newUser.IsLoggedIn;
            dbUser.LastLoggedInDate = newUser.LastLoggedInDate;

            await context.SaveChangesAsync(token);
        }

        public async Task SaveStepAsync(Step newStep, CancellationToken token)
        {
            var dbStep = await context.Steps.FirstOrDefaultAsync(s => s.Id == newStep.Id, token);
            if (dbStep == null)
            {
                await context.Steps.AddAsync(newStep, token);
                await context.SaveChangesAsync(token);
                return;
            }

            var dbMode = await context.Modes.FirstOrDefaultAsync(m => m.Id == newStep.ModeId);
            if (dbMode != null)
            {
                dbStep.Mode = dbMode;
            }


            dbStep.Id = newStep.Id;
            dbStep.Volume = newStep.Volume;
            dbStep.Timer = newStep.Timer;
            dbStep.Destination = newStep.Destination;
            dbStep.ModeId = newStep.ModeId;
            dbStep.Speed = newStep.Speed;
            dbStep.Type = newStep.Type;
            dbStep.User = newStep.User;

            await context.SaveChangesAsync(token);
        }

        public async Task SaveStepsAsync(List<Step> newSteps, CancellationToken token)
        {

            if (!await context.Steps.AnyAsync(token))
            {
                await context.Steps.AddRangeAsync(newSteps, token);
                await context.SaveChangesAsync(token);
                return;
            }

            var updatedSteps = newSteps.Except(context.Steps);

            //NOTE: если список новых шагов и таблица шагов в базе одинаковые, то ничего не делаем
            if (!updatedSteps.Any())
            {
                return;
            }

            foreach (var step in updatedSteps)
            {
                await SaveStepAsync(step, token);
            }
        }

        public async Task SaveModeAsync(Mode newMode, CancellationToken token)
        {
            var dbMode = await context.Modes.FirstOrDefaultAsync(m => m.Id == newMode.Id, token);


            if (dbMode == null)
            {
                await context.Modes.AddAsync(newMode, token);
                await context.SaveChangesAsync(token);
                return;
            }

            dbMode.Id = newMode.Id;
            dbMode.Name = newMode.Name;
            dbMode.MaxBottleNumber = newMode.MaxBottleNumber;
            dbMode.MaxUsedTips = newMode.MaxUsedTips;
            dbMode.Users = newMode.Users;

            await context.SaveChangesAsync(token);
        }

        public async Task SaveModesAsync(List<Mode> newModes, CancellationToken token)
        {
            if (!await context.Modes.AnyAsync(token))
            {
                await context.Modes.AddRangeAsync(newModes, token);
                await context.SaveChangesAsync(token);
                return;
            }

            var updatedModes = newModes.Except(context.Modes);

            //NOTE: если список новых модов и таблица модов в базе одинаковые, то ничего не делаем
            if (!updatedModes.Any())
            {
                return;
            }
            foreach (var mode in updatedModes)
            {
                await SaveModeAsync(mode, token);
            }
        }

    }
}
