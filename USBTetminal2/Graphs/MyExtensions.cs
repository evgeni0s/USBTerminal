using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace USBTetminal2.Graphs
{
    public static class MyExtensions
    {
        /// <summary>
        /// Composes CompositeDataSource instance from x array and y array
        /// Example. CompositeDataSource chartData = x.ToChartDataSource(y) 
        /// </summary>
        /// <param name="arrayX">array of double X</param>
        /// <param name="arrayY">array of double Y</param>
        /// <returns>Class which is the must for bilding charts using Dynamic Data Display library</returns>
        public static CompositeDataSource ToChartDataSource(this IEnumerable<double> arrayX, IEnumerable<double> arrayY)
        {
            CompositeDataSource result = new CompositeDataSource();
            if (arrayX.Count() != arrayY.Count() || arrayX.Count() == 0 || arrayY.Count() == 0)
            {
                MessageBox.Show(String.Format("Array inconsistency: X array contains {0} elements, while Y array contains {1}",
                    arrayX.Count(), arrayY.Count()));
            }
            var xData = new EnumerableDataSource<double>(arrayX);
            xData.SetXMapping(x => x);
            var yData = new EnumerableDataSource<double>(arrayY);
            yData.SetYMapping(y => y);
            result = xData.Join(yData);
            return result;
        }

        public static EnumerableDataSource<Point> GetPoints(this ChartPlotter plotter, LineGraph graph)
        {
            return (EnumerableDataSource<Point>)graph.DataSource;
        }
        //Worked long ago
        //public static void DefaultSettings(this ChartPlotter plotter)
        //{
        //    plotter.Viewport.AutoFitToView = true;
        //    GraphAxisHelper.ViewportAxesRangeRestriction restr = new GraphAxisHelper.ViewportAxesRangeRestriction();
        //    restr.XRange = new GraphAxisHelper.DisplayRange(0, 66000, GraphAxisHelper.DisplayRange.DisplayRangeType.Min);
        //    restr.YRange = new GraphAxisHelper.DisplayRange(0, 66000, GraphAxisHelper.DisplayRange.DisplayRangeType.Min);
        //    plotter.Viewport.Restrictions.Add(restr);
        //}

        public static void DefaultSettings(this ChartPlotter plotter)
        {
            plotter.Viewport.AutoFitToView = true;
            GraphAxisHelper.ViewportAxesRangeRestriction restr = new GraphAxisHelper.ViewportAxesRangeRestriction();
            restr.XRange = new GraphAxisHelper.DisplayRange(0, 200, GraphAxisHelper.DisplayRange.DisplayRangeType.Min);
            restr.YRange = new GraphAxisHelper.DisplayRange(300, 10000, GraphAxisHelper.DisplayRange.DisplayRangeType.Min);
            plotter.Viewport.Restrictions.Add(restr);
        }
    }
}
