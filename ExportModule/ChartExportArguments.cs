using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExportModule
{
    //Not used for now
    //I've created this class to export more complex data like line collor and lables
    public class ChartExportArguments
    {
        public IEnumerable<Point> Points { get; set; }
        //public DataSet ChartData { get; set; }
        public string ChartId { get; set; }

        //graph.LineGraph.Stroke as SolidColorBrush;
        //_graph.LineGraph.DataSource.GetPoints();
    }
}
