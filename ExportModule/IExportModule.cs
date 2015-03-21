using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ExportModule
{
    public interface IExportModule
    {
        //void ShowFolderBrowserView(Action<FileInfo> selectedPathCallback);

        void ShowFolderBrowserView(Action<string> selectedPathCallback);
        //void ChooseExportTypeView(Action<EExportType> selectedTypeCallback);
        //void Export(List<Point> points);

        //void Export(List<LineGraph> LineGraph);
        void Export(List<ChartExportArguments> exportData);

    }
}
