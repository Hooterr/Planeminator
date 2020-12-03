using Microsoft.Win32;
using Planeminator.DesktopApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planeminator.DesktopApp.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string BrowseForFile(string filter)
        {
            return BrowseForFileNative(filter);
        }

        public string BrowseForFile()
        {
            return BrowseForFileNative();
        }

        private string BrowseForFileNative(string filter = null)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter
            };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }
    }
}
