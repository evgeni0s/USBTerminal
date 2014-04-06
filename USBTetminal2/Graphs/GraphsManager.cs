using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using USBTetminal2.Commands;

namespace USBTetminal2.Graphs
{
    /// <summary>
    /// Use this to manipulate with existing graphs. 
    /// read Data
    /// change Lines visibility, 
    /// access to Markers. 
    /// Markers - ??? ))) points on graph or legend shit. Don't remember)
    /// </summary>
    public class GraphsManager: ISimpleBroadcastListener
    {
        List<LineAndMarker<MarkerPointsGraph>> GraphsCollection = new List<LineAndMarker<MarkerPointsGraph>>();
        ChartPlotter _plotter;
        Random hue = new Random(255); //Part of HSV spectrum
        public GraphsManager(ChartPlotter plotter)
        {
            _plotter = plotter;
        }

        public void ReciveMessage(Grahps.CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case USBTetminal2.Grahps.CommonBroadcastType.DECODE_BYTE_ARRAY_FROM_DEVICE:
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS:
                    buildGraphFromYPoints(data);
                    break;
                case USBTetminal2.Grahps.CommonBroadcastType.msg3:
                    break;
                default:
                    break;
            }
        }



        #region private methods
        //Colverts object data -> List<double>
        private void buildGraphFromYPoints(object data)
        {
            if (data.GetType() == typeof(List<int>))
            {
                List<double> yPoints = ((List<int>)data).Select(i => (double)i).ToList();
                buildGraphFromYPoints(yPoints);
            }
            else if (data.GetType() == typeof(List<double>))
            {
                buildGraphFromYPoints((List<double>)data);
            }

        }

        private void buildGraphFromYPoints(List<double> yPoints)
        {
            List<double> xPoints = generateXArray(yPoints);

            //Dymamic Data  Dispaly works with this source
            CompositeDataSource dataSource = xPoints.ToChartDataSource(yPoints);
            buildGraphFromDataSource(dataSource);

        }

        private void buildGraphFromDataSource(CompositeDataSource dataSource)
        {
            var graph = _plotter.AddLineGraph(dataSource, new Pen(randomBrush(), 2), new CirclePointMarker { Size = 5, Fill = Brushes.Red }, new PenDescription("High Limit"));
            GraphsCollection.Add(graph);
        }

        /// <summary>
        /// Creates brushes with high value. Such brushes can be well seeen on white sheet
        /// </summary>
        /// <returns></returns>
        private SolidColorBrush randomBrush()
        {
            SolidColorBrush result = new SolidColorBrush();
            Color wellVisibleColor = ColorFromHSV(hue.Next(), 1, 1);
            result.Color = wellVisibleColor;
            return result;
        }


        /// <summary>
        /// Manualy converts RGB to HSV
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation">From 0 to 1</param>
        /// <param name="value">From 0 to 1</param>
        /// <returns></returns>
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - saturation));
            byte q = Convert.ToByte(value * (1 - f * saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * saturation));

            if (hi == 0)

                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }


        /// <summary>
        /// Currently I use x = y function
        /// More info at http://research.microsoft.com/en-us/um/cambridge/projects/ddd/d3isdk/
        /// Can make double instead of int
        /// </summary>
        /// <param name="yPoints"></param>
        /// <returns></returns>
        private List<double> generateXArray(List<double> yPoints)
        {

            // Compute x array of 1001 points from 0 to 100 with 1 step
            double[] x = Enumerable.Range(0, yPoints.Count).Select(i => i/1.0).ToArray();
            return new List<double>(x);
        }

        //NOT USED FOR NOW!!!!!!!
        /// <summary>
        /// Currently I use x = y function
        /// More info at http://research.microsoft.com/en-us/um/cambridge/projects/ddd/d3isdk/
        /// Can make double instead of int
        /// </summary>
        /// <param name="yPoints"></param>
        /// <returns></returns>
        private List<int> generateXArray(List<int> yPoints)
        {

            // Compute x array of 1001 points from 0 to 100 with 1 step
            int[] x = Enumerable.Range(0, yPoints.Count).Select(i => i).ToArray();
            return new List<int>(x);
        }


        #endregion


        #region for debug only
        private void showPoints(object data)
        {
            List<int> yPoints = (List<int>)data;
            log("GraphsManager: Recived Points!");
            foreach (int y in yPoints)
            {
                log(y.ToString());
            }
        }


        private void log(string logMsg)
        {
            CustomCommands.ErrorReport.Execute(logMsg, App.Current.MainWindow);
        }

        #endregion
    }
}
