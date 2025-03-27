using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrackerDesktop.Data.Entities;
using TrackerDesktop.Data.Services;
using TrackerDesktop.Mappers;
using TrackerDesktop.Services;

namespace TrackerDesktop.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        private string filePath = string.Empty;
        private readonly string modesExcelWorkSheetName = "Modes";
        private readonly string stepsExcelWorkSheetName = "Steps";
        private readonly IDialogService dialogService;
        private readonly ITrackerDatabaseService databaseService;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        [ObservableProperty]
        private User? currentUser = new User();

        [ObservableProperty]
        private List<string> excelWorkSheetNames = new List<string>();

        [ObservableProperty]
        private string selectedExcelWorkSheetName = string.Empty;

        [ObservableProperty]
        private bool isModesVisible = false;

        [ObservableProperty]
        private bool isStepsVisible = false;

        [ObservableProperty]
        private bool isWorkSheetNamesVisible = false;

        private ObservableCollection<Mode> modes;

        public ObservableCollection<Mode> Modes
        {
            get
            {
                return modes;
            }
            set
            {
                if (modes == value)
                {
                    return;
                }
                modes = value;
                OnPropertyChanged(nameof(Modes));
            }
        }

        private ObservableCollection<Step> steps;

        public ObservableCollection<Step> Steps
        {
            get
            {
                return steps;
            }
            set
            {
                if (steps == value)
                {
                    return;
                }
                steps = value;
                OnPropertyChanged(nameof(Steps));
            }
        }

        public HomePageViewModel(IDialogService dialogService,
                                 ITrackerDatabaseService databaseService)
        {
            this.dialogService = dialogService;
            this.databaseService = databaseService;
            ExcelWorkSheetNames.Add(modesExcelWorkSheetName);
            ExcelWorkSheetNames.Add(stepsExcelWorkSheetName);
        }

        [RelayCommand]
        public async Task OpenExcelFileAsync()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow is not null)
            {
                var storage = desktop.MainWindow.StorageProvider;

                filePath = await dialogService.OpenFileAsync(storage);

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return;
                }
            }

            var (modes, steps) = ParseExcelFile();

            if (!modes.Any() && !steps.Any())
            {
                await dialogService.ShowMessage("Excel file must contain Modes and Steps worksheets!");
                return;
            }

            //currentUser = await databaseService.GetLastLoggedInUserAsync(cancellationTokenSource.Token);

            foreach (var step in steps)
            {
                var mode = modes.FirstOrDefault(x => x.Id == step.ModeId);
                if (mode != null)
                {
                    step.Mode = mode;
                    if (CurrentUser != null)
                    {
                        //step.User.Add(currentUser);
                        step.User = CurrentUser;
                    }
                }
            }

            foreach (var mode in modes)
            {
                if (CurrentUser != null)
                {
                    mode.Users?.Add(CurrentUser);
                }
            }

            await databaseService.SaveModesAsync(modes, cancellationTokenSource.Token);
            await databaseService.SaveStepsAsync(steps, cancellationTokenSource.Token);

            Modes = new ObservableCollection<Mode>(modes);
            Steps = new ObservableCollection<Step>(steps);

            IsWorkSheetNamesVisible = true;
        }

        [RelayCommand]
        public async Task SaveExcelFileAsync()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow is not null)
            {
                var storage = desktop.MainWindow.StorageProvider;

                var res = await dialogService.SaveFileAsync(storage);

                if (res == null)
                {
                    await dialogService.ShowMessage("Operation was cancelled");
                    return;
                }

                using var workbook = new XLWorkbook();

                var modes = ExcelMappers.MapModesToExcelModes(Modes);
                var steps = ExcelMappers.MapStepsToExcelSteps(Steps);

                workbook.AddWorksheet(modesExcelWorkSheetName).FirstCell().InsertTable(modes, false);
                workbook.AddWorksheet(stepsExcelWorkSheetName).FirstCell().InsertTable(steps, false);


                workbook.SaveAs(res.Path.AbsolutePath);
            }
        }

        [RelayCommand]
        public void ComboBoxItemChanged()
        {
            if (SelectedExcelWorkSheetName == modesExcelWorkSheetName)
            {
                IsModesVisible = true;
                IsStepsVisible = false;
                return;
            }
            else if (SelectedExcelWorkSheetName == stepsExcelWorkSheetName)
            {
                IsModesVisible = false;
                IsStepsVisible = true;
                return;
            }
            else
            {
                return;
            }
        }

        public (List<Mode> modes, List<Step> steps) ParseExcelFile()
        {

            using var workbook = new XLWorkbook(filePath);

            var excelModes = new List<Mode>();
            var excelSteps = new List<Step>();

            foreach (var sheet in workbook.Worksheets)
            {
                if (sheet.IsEmpty())
                {
                    continue;
                }

                if (sheet.Name == modesExcelWorkSheetName)
                {
                    // NOTE: Пропускаем 1-ую строку с названиями столбцов
                    for (int i = 2; i <= sheet.RowCount(); i++)
                    {
                        if (sheet.Row(i).IsEmpty())
                        {
                            break;
                        }

                        var excelMode = new Mode
                        {
                            Id = sheet.Cell(i, 1).GetValue<int>(),
                            Name = sheet.Cell(i, 2).GetString(),
                            MaxBottleNumber = sheet.Cell(i, 3).GetValue<int>(),
                            MaxUsedTips = sheet.Cell(i, 4).GetValue<int>(),

                        };
                        excelModes.Add(excelMode);
                    }

                }
                else if (sheet.Name == stepsExcelWorkSheetName)
                {
                    for (int i = 2; i <= sheet.RowCount(); i++)
                    {
                        if (sheet.Row(i).IsEmpty())
                        {
                            break;
                        }

                        var excelStep = new Step
                        {
                            Id = sheet.Cell(i, 1).GetValue<int>(),
                            ModeId = sheet.Cell(i, 2).GetValue<int>(),
                            Timer = sheet.Cell(i, 3).GetValue<int>(),
                            Destination = sheet.Cell(i, 4).GetString(),
                            Speed = sheet.Cell(i, 5).GetValue<int>(),
                            Type = sheet.Cell(i, 6).GetString(),
                            Volume = sheet.Cell(i, 7).GetValue<int>(),
                        };
                        excelSteps.Add(excelStep);
                    }
                }
                else
                {
                    return (modes: new List<Mode>(), steps: new List<Step>());
                }
            }

            return (modes: excelModes, steps: excelSteps);
            
        }

        [RelayCommand]
        public async Task SaveChangesAsync()
        {
            await databaseService.SaveModesAsync(Modes.ToList(), cancellationTokenSource.Token);
            await databaseService.SaveStepsAsync(Steps.ToList(), cancellationTokenSource.Token);
        }

        [RelayCommand]
        public async Task LogOutAsync()
        {
            CurrentUser.IsLoggedIn = false;
            await databaseService.SaveUserAsync(CurrentUser, cancellationTokenSource.Token);
        }

        [RelayCommand]
        public async Task PageLoadedAsync()
        {
            CurrentUser = await databaseService.GetLastLoggedInUserAsync(cancellationTokenSource.Token);

        }
    }
}
