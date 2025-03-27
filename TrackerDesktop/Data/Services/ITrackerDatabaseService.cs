using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackerDesktop.Data.Entities;

namespace TrackerDesktop.Data.Services
{
    public interface ITrackerDatabaseService
    {
        Task<User?> GetLastLoggedInUserAsync(CancellationToken token);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken token);
        Task<User?> GetUserByNickNameAsync(string nickName, CancellationToken token);
        Task SaveUserAsync(User newUser, CancellationToken token);
        Task SaveStepAsync(Step newStep, CancellationToken token);
        Task SaveStepsAsync(List<Step> newSteps, CancellationToken token);
        Task SaveModeAsync(Mode newMode, CancellationToken token);
        Task SaveModesAsync(List<Mode> newModes, CancellationToken token);
    }
}