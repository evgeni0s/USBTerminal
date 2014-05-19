using System;
namespace Excel.Services
{
    interface IExcelService
    {
        void AddItem(string id);
        void Export();
        void ExportTo();
    }
}
