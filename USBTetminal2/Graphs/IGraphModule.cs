using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace USBTetminal2.Graphs
{
    public interface IGraphModule
    {
        void show();
        void buildGraphFromYPoints(object data);
        void changeMarkersVisibility();
        void clearAll();
        void removeGraph(object data);
        void showLegend();
        //ICommand ClearCommand 
        //event EventHandler OnActivate;
        
    }
}
