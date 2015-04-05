using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
namespace ExportModule.Services
{
    //this service becomes available after Export module is initialized
    //Do not inject it to for view models outside this module
    public interface IExcelService
    {
        bool CreateExcelFile(string path);

        void ExportToExcelFile(string path, List<ChartExportArguments> args);


    }
}
