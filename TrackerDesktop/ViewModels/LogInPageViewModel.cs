using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;
using TrackerDesktop.Data.Services;

namespace TrackerDesktop.ViewModels
{
    public partial class LogInPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string password = string.Empty;
        [ObservableProperty]
        private string notifyLabel = string.Empty;

        private readonly ITrackerDatabaseService database;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [ObservableProperty]
        private string successNotifyLabel = string.Empty;

        public LogInPageViewModel(ITrackerDatabaseService database)
        {
            this.database = database;
        }

        [RelayCommand]
        public async Task LogInAsync()
        {
            await LogIn();
        }


        public async Task LogIn()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                NotifyLabel = "Email is empty!";
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                NotifyLabel = "Password in empty!";
                return;
            }
            var user = await database.GetUserByEmailAsync(Email, cancellationTokenSource.Token);

            if (user != null)
            {
                bool isPasswordVerified = BCrypt.Net.BCrypt.Verify(Password, user.Password);
                if (!isPasswordVerified)
                {
                    NotifyLabel = "Incorrect email or password!";
                    return;
                }

                user.IsLoggedIn = true;
                await database.SaveUserAsync(user, cancellationTokenSource.Token);

            }
            else
            {
                NotifyLabel = "Incorrect email or password!";
                return;
            }

            NotifyLabel = string.Empty;
            SuccessNotifyLabel = "Authorization completed successfully!";

            Email = string.Empty;
            Password = string.Empty;
            NotifyLabel = string.Empty;
        }

    }
}
