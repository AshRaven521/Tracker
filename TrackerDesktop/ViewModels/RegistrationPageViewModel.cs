using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TrackerDesktop.Data.Entities;
using TrackerDesktop.Data.Services;

namespace TrackerDesktop.ViewModels
{
    public partial class RegistrationPageViewModel : ViewModelBase
    {
        private string email = string.Empty;
        private string password = string.Empty;
        [ObservableProperty]
        private string nickName = string.Empty;
        [ObservableProperty]
        private string notifyLabel = string.Empty;
        [ObservableProperty]
        private string passwordEntryColor = "WhiteSmoke";
        [ObservableProperty]
        private string nicknameEntryColor = "WhiteSmoke";
        [ObservableProperty]
        private string emailEntryColor = "WhiteSmoke";
        private readonly ITrackerDatabaseService database;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [ObservableProperty]
        private string successNotifyLabel = string.Empty;

        public RegistrationPageViewModel(ITrackerDatabaseService database)
        {
            NotifyLabel = string.Empty;
            this.database = database;
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email == value)
                {
                    return;
                }
                email = value;
                OnPropertyChanged();
                EmailChanged();
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password == value)
                {
                    return;
                }
                password = value;
                OnPropertyChanged();
                PasswordChanged();
            }
        }

        [RelayCommand]
        public async Task RegisterAsync()
        {
            await Register();
        }

        [RelayCommand]
        public void PasswordChanged()
        {
            PasswordEntryChanged();
        }

        [RelayCommand]
        public void EmailChanged()
        {
            EmailEntryChanged();
        }

        public async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                NotifyLabel = "Email is empty!";
                EmailEntryColor = "Red";
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                NotifyLabel = "Password is empty!";
                PasswordEntryColor = "Red";
                return;
            }
            if (string.IsNullOrWhiteSpace(NickName))
            {
                NotifyLabel = "NickName is empty!";
                NicknameEntryColor = "Red";
                return;
            }
            if (await database.GetUserByEmailAsync(Email, cancellationTokenSource.Token) != null)
            {
                NotifyLabel = "Email is not available!";
                EmailEntryColor = "Red";
                return;
            }
            if (await database.GetUserByNickNameAsync(NickName, cancellationTokenSource.Token) != null)
            {
                NotifyLabel = "Nickname is not available!";
                NicknameEntryColor = "Red";
                return;
            }
            if (!IsValidPassword(Password))
            {
                PasswordEntryColor = "Red";
                NotifyLabel = "Invalid password format! Password must contain: \n·At least 8 characters\n·One uppercase letter\n·One lowercase letter\n·One digit\n·One special character";
                return;
            }
            if (!IsValidEmail(Email))
            {
                EmailEntryColor = "Red";
                NotifyLabel = "Invalid email format!";
                return;
            }
            if (!IsValidNickname(NickName))
            {
                PasswordEntryColor = "Red";
                NotifyLabel = "Invalid nickname format! Nickname must contain: \n·At least 3 characters\n·Only letters,digits and _";
                return;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            var user = new User(NickName, hashedPassword, Email, true, DateTime.Now);
            await database.SaveUserAsync(user, cancellationTokenSource.Token);

            NotifyLabel = string.Empty;
            SuccessNotifyLabel = "Registration completed successfully!";

            NickName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;

        }
        public void PasswordEntryChanged()
        {
            if (!IsValidPassword(Password))
            {
                PasswordEntryColor = "Red";
                NotifyLabel = "Invalid password format! Password must contain: \n·At least 8 characters\n·One uppercase letter\n·One lowercase letter\n·One digit\n·One special character";
            }
            else
            {
                PasswordEntryColor = "Green";
                NotifyLabel = string.Empty;
            }
        }
        public void EmailEntryChanged()
        {
            if (!IsValidEmail(Email))
            {
                EmailEntryColor = "Red";
                NotifyLabel = "Invalid email format!";
            }
            else
            {
                EmailEntryColor = "Green";
                NotifyLabel = string.Empty;
            }
        }
        public void NicknameEntryChanged()
        {
            if (!IsValidNickname(NickName))
            {
                EmailEntryColor = "Red";
                NotifyLabel = "Invalid nickname format! Nickname must contain: \n·At least 3 characters\n·Only letters,digits and _";
            }
            else
            {
                EmailEntryColor = "Green";
                NotifyLabel = string.Empty;
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, pattern);
        }
        private bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        private bool IsValidNickname(string nickname)
        {
            if (nickname.Length < 3 || nickname.Length > 18)
            {
                return false;
            }
            foreach (char c in nickname)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
