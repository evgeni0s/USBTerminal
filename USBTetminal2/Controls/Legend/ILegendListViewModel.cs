using System;
using USBTetminal2.Graphs;
namespace USBTetminal2.Controls.Legend
{
    public interface ILegendListViewModel
    {
        void addNewLegend(object data);
        void clearAll();
        void deleteLegend(object data);
        void switchLegendVisibility(object cmdParam = null);
        void SetGraph(IGraphModule graph);
    }
}
