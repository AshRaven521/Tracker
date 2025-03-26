using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TrackerDesktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ViewModelBase? currentPage;

        private readonly LogInPageViewModel logInPage;
        private readonly RegistrationPageViewModel registrationPage;
        private readonly HomePageViewModel homePage;

        public MainWindowViewModel(LogInPageViewModel logInPage,
                                   RegistrationPageViewModel registrationPage,
                                   HomePageViewModel homePage)
        {
            this.logInPage = logInPage;
            this.registrationPage = registrationPage;
            this.homePage = homePage;
            CurrentPage = null;
        }

        [RelayCommand]
        public void GoToLogInPage()
        {
            CurrentPage = logInPage;
        }

        [RelayCommand]
        public void GoRegistrationPage()
        {
            CurrentPage = registrationPage;
        }

        [RelayCommand]
        public void GoToHomePage()
        {
            CurrentPage = homePage;
        }

    }
}
