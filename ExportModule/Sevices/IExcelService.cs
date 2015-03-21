using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
namespace ExportModule.Services
{
    //this service becomes available after Export module is initialized
    //Do not inject it to for view models outside this module
    public interface IExcelService
    {
        //void AddItem(string id);
        //void Export();
        //void ExportTo();

        /// <returns>result true if document was successfully exported</returns>
        //bool CreateDocument(List<LineGraph> selectedGraphs, string path);
        bool CreateExcelFile(string path);
        void ExportToExcelFile(string path);
    }
}
