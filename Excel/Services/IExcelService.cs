using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
namespace Excel
{
    public interface IExcelService
    {
        void AddItem(string id);
        void Export();
        void ExportTo();

        /// <returns>result true if document was successfully exported</returns>
        bool CreateDocument(List<LineGraph> selectedGraphs, string path);
    }
}
