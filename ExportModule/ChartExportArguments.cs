using Infrastructure;
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
    public class ChartExportArguments : ViewModelBase
    {
        private string _xName = "msec";
        public string XName
        {
            get { return _xName; }
            set { _xName = value; }
        }

        private string _yName = "values";
        public string YName
        {
            get { return _yName; }
            set { _yName = value; }
        }
        
        private int _width = 500;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private int height = 300;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int _xOffset_Chart = 100;
        public int XOffset_Chart
        {
            get { return _xOffset_Chart; }
            set { _xOffset_Chart = value; }
        }

        private int _yOffset_Chart = 100;
        public int YOffset_Chart
        {
            get { return _yOffset_Chart; }
            set { _yOffset_Chart = value; }
        }

        private bool _xAxis_HasTitle = true;
        public bool XAxis_HasTitle
        {
            get { return _xAxis_HasTitle; }
            set { _xAxis_HasTitle = value; }
        }

        private string _xAxis_AxisTitle = "X Axis";
        public string XAxis_AxisTitle
        {
            get { return _xAxis_AxisTitle; }
            set { _xAxis_AxisTitle = value; }
        }

        private bool _yAxis_HasTitle = true;
        public bool YAxis_HasTitle
        {
            get { return _yAxis_HasTitle; }
            set { _yAxis_HasTitle = value; }
        }

        private string _yAxis_AxisTitle = "Y Axis";
        public string YAxis_AxisTitle
        {
            get { return _yAxis_AxisTitle; }
            set { _yAxis_AxisTitle = value; }
        }

        private bool _hasTitle = true;
        public bool HasTitle
        {
            get { return _hasTitle; }
            set { _hasTitle = value; }
        }

        private string _chartTitle = "Project Status Graph";
        public string ChartTitle
        {
            get { return _chartTitle; }
            set { _chartTitle = value;
            OnPropertyChanged("ChartTitle");
            }
        }

        private bool _hasLegend = true;
        public bool HasLegend
        {
            get { return _hasLegend; }
            set { _hasLegend = value; }
        }

        public List<Point> Points { get; set; }

        //All parans except Name/Id can be reasigned. And points remain same
        public void CopyOptionsFromOtherChart(ChartExportArguments source)
        {
            if (source == null) return;
            this.HasTitle = source.HasTitle;
            this.Height = source.Height;
            this.Width = source.Width;
            this.XAxis_AxisTitle = source.XAxis_AxisTitle;
            this.YAxis_AxisTitle = source.YAxis_AxisTitle;
            this.HasLegend = source.HasLegend;
            this.XName = source.XName; 
            this.YName = source.YName;
            this.XOffset_Chart = source.XOffset_Chart;
            this.YOffset_Chart = source.YOffset_Chart;
            this.XAxis_HasTitle = source.XAxis_HasTitle;
            this.YAxis_HasTitle = source.YAxis_HasTitle;
        }


        //public override string ToString()
        //{
        //    return ChartTitle;
        //}
        //public DataSet ChartData { get; set; }
        //public string ChartId { get; set; }

        //graph.LineGraph.Stroke as SolidColorBrush;
        //_graph.LineGraph.DataSource.GetPoints();
    }
}
