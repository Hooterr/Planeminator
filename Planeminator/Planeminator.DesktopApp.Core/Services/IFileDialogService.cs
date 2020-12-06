using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.DesktopApp.Core.Services
{
    public interface IFileDialogService
    {
        string BrowseForFile(string filter);
        string BrowseForFile();

        string SaveToFile(string filter);
        string SaveToFile();
    }
}
