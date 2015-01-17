using System;
namespace Excel
{
    interface IExcelService
    {
        void AddItem(string id);
        void Export();
        void ExportTo();
    }
}
