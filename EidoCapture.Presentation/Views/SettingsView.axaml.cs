using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using EidoCapture.Presentation.Base;
using EidoCapture.Presentation.Base.Extensions;
using EidoCapture.Presentation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EidoCapture.Presentation.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            this.SetDataContext(App.Current?.Services);
        }

        private void SelectFolderBtn_OnClick(object? sender, RoutedEventArgs a)
        {
            TopLevel? topLevel = TopLevel.GetTopLevel(this);

            if (topLevel != null)
            {
                IStorageProvider storageProvider = topLevel.StorageProvider;

                IReadOnlyList<IStorageFolder> folders = Task.Run(() => storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions() { AllowMultiple = false })).Result;

                IStorageFolder? folder = folders.FirstOrDefault();

                if (folder != null)
                {
                    string folderPath = folder.Path.AbsolutePath;

                    if (!string.IsNullOrEmpty(folderPath) && DataContext is SettingsViewModel viewModel)
                    {
                        viewModel.StorageFolderPath = folderPath;
                    }
                }
            }
        }

        private void PrivacyBtn_OnClick(object? sender, RoutedEventArgs a)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                Browser.Open(viewModel.PrivacyPolicyURL);
            }
        }

        private void IssuesBtn_OnClick(object? sender, RoutedEventArgs a)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                Browser.Open(viewModel.IssuesURL);
            }
        }
    }
}
