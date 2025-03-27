using Avalonia.Platform.Storage;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Threading.Tasks;

namespace TrackerDesktop.Services
{
    public class DialogService : IDialogService
    {
        private static FilePickerFileType Excel { get; } = new("All xls")
        {
            Patterns = new[]
            {
                "*.xlsx",
                "*.xls"
            },
            MimeTypes = null,
            AppleUniformTypeIdentifiers = null,
        };

        public async Task<string> OpenFileAsync(IStorageProvider storage)
        {
            var options = new FilePickerOpenOptions
            {
                Title = "Open Excel data file",
                AllowMultiple = false,
                FileTypeFilter = new[] { Excel },
            };

            var openedFiles = await storage.OpenFilePickerAsync(options);
            if (openedFiles.Count >= 1)
            {
                return openedFiles[0].Path.AbsolutePath;
            }
            return string.Empty;
        }

        public async Task<IStorageFile?> SaveFileAsync(IStorageProvider storage)
        {
            var options = new FilePickerSaveOptions
            {
                Title = "Open Excel data file",
                FileTypeChoices = new[] { Excel },
            };

            var result = await storage.SaveFilePickerAsync(options);

            return result;
        }

        public async Task ShowMessage(string message)
        {
            var box = MessageBoxManager
                            .GetMessageBoxStandard("Info", message,
                                ButtonEnum.Ok);

            var result = await box.ShowAsync();
        }
    }
}
