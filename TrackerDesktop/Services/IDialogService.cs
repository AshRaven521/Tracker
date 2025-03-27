using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace TrackerDesktop.Services
{
    public interface IDialogService
    {
        Task<string> OpenFileAsync(IStorageProvider storage);
        Task<IStorageFile?> SaveFileAsync(IStorageProvider storage);
        Task ShowMessage(string message);
    }
}