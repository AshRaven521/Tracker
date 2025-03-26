using Avalonia.Platform.Storage;
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
            //var openFileDialog = new OpenFilePicker

            //openFileDialog.Filter = "Json files (*.json)|*.json";
            //openFileDialog.Title = "Выберите файл конфигурации";
            //openFileDialog.Multiselect = false;

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    return openFileDialog.FileName;
            //}
            //return string.Empty;

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

        public async Task SaveFileAsync(IStorageProvider storage)
        {
            var options = new FilePickerSaveOptions
            {
                Title = "Open Excel data file",
                FileTypeChoices = new[] { Excel },
            };

            await storage.SaveFilePickerAsync(options);
        }
    }
}
