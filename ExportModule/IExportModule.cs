using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportModule
{
    public interface IExportModule
    {
        //void ShowFolderBrowserView(Action<FileInfo> selectedPathCallback);

        void ShowFolderBrowserView(Action<string> selectedPathCallback);
    }
}
