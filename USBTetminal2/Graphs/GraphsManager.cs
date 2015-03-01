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
using System.Windows.Data;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using USBTetminal2.Controls.Legend;
using USBTetminal2.Controls.Chart;
using Microsoft.Practices.Prism.Logging;

namespace USBTetminal2.Graphs
{
    /// <summary>
    /// Use this to manipulate with existing graphs. 
    /// read Data
    /// change Lines visibility, 
    /// access to Markers. 
    /// Markers - ??? ))) points on graph or legend shit. Don't remember)
    /// </summary>
    public class GraphsManager : IModule, IGraphModule//: ISimpleBroadcastListener
    {
        private List<LineAndMarker<MarkerPointsGraph>> GraphsCollection = new List<LineAndMarker<MarkerPointsGraph>>();
        private ChartPlotter _plotter;         //Group A
        private ChartView _chartView;          //Group A
        private ChartViewModel _chartViewModel;//Group A
        private Random hue = new Random(255); //Part of HSV spectrum 
        const Visibility DEFAULT_MARKER_VILIBILITY = Visibility.Collapsed;
        private IRegionManager _regionManager;
        private IViewModelProvider _viewModelProvider;
        private ILegendListViewModel _legendListViewModel;
        private ILoggerFacade _logger;
        public static List<int> TestData = new List<int> {506, 506, 506, 507, 507, 507, 507, 508, 508, 509, 509, 509, 510, 510, 510,
            511, 511, 512, 512, 512, 513, 513, 514, 514, 514, 515, 515, 516, 516, 516, 518, 522, 525, 529, 532,
            536, 540, 543, 547, 551 ,554 ,558 ,561 ,565 ,568 ,572 ,576 ,579 ,583 ,587 ,590, 594 ,597 ,601, 604 ,
            608, 611, 630, 664, 698, 731, 764, 796, 828, 860, 891, 921, 951, 981, 1011, 1040, 1068, 1097, 1125, 1152, 1180, 1206, 1233, 1259,
            1285, 1311, 1336, 1361, 1386, 1514, 1728, 1920, 2091, 2244, 2381 ,2503 ,2613 ,2712 ,2802 ,2882 ,2954 ,3040 ,3189 ,3229 ,3265 ,
            3297 ,3326 ,3352 ,3374 ,3396 ,3414, 3429 ,3442 ,3455 ,3466 ,3476 ,3533 ,3584 ,3600 ,3612 ,3614 ,3615 ,3615 ,506 ,3615};

        public GraphsManager(IRegionManager regionManager, IViewModelProvider viewModelProvider, ILoggerFacade logger)
        {
            _regionManager = regionManager;
            _viewModelProvider = viewModelProvider;
            _logger = logger;
            _legendListViewModel = _viewModelProvider.GetViewModel<LegendListViewModel>("LegendListKey");
            _legendListViewModel.SetGraph(this);
            _chartViewModel = _viewModelProvider.GetViewModel<ChartViewModel>(_legendListViewModel);
            _chartView = new ChartView() { DataContext = _chartViewModel };
            _plotter = _chartView.mPlotter;
            _plotter.LegendVisible = false;
            _plotter.DefaultSettings();
            hideDefaultLegend();//Disabled default ledend so that I could use my custom


        }




        public void ReciveMessage(CommonBroadcastType smgType, object data)
        {
            switch (smgType)
            {
                case CommonBroadcastType.DELETE_GRAPH:
                    removeGraph(data);
                    break;
                case CommonBroadcastType.BUILD_GRAPH_FROM_Y_POINTS:
                    buildGraphFromYPoints(data);
                    break;
                case CommonBroadcastType.CLEAR_ALL:
                    clearAll();
                    break;
                case CommonBroadcastType.CHANGE_MARKERS_VISIBILITY:
                    changeMarkersVisibility();
                    break;
                default:
                    break;
            }
        }






        #region private methods

        /// <summary>
        /// Needs to be updated regulary
        /// </summary>
        public void hideDefaultLegend()
        {
            _plotter.Legend.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// try to show all markers who have visible solid lines
        /// if they are already visible - hide all
        /// </summary>
        public void changeMarkersVisibility()
        {
            bool allMarkersAreVisible = GraphsCollection.Where(pair => pair.LineGraph.Visibility == Visibility.Visible).//for thouse graphs who's  solid lines are visible,
                                                         All(pair => pair.MarkerGraph.Visibility == Visibility.Visible);//check if all their markers are visible as well
            if (allMarkersAreVisible)
            {
                //GraphsCollection.Where(pair => pair.LineGraph.Visibility == Visibility.Visible)
                //                .Select(pair => pair.MarkerGraph.Visibility = Visibility.Collapsed);

                foreach (var pair in GraphsCollection)
                {
                    if (pair.LineGraph.Visibility == Visibility.Visible)
                    {
                        pair.MarkerGraph.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else 
            {
                foreach (var pair in GraphsCollection)
                {
                    if (pair.LineGraph.Visibility == Visibility.Visible)
                    {
                        pair.MarkerGraph.Visibility = Visibility.Visible;
                    }
                }
                //GraphsCollection.Where(pair => pair.LineGraph.Visibility == Visibility.Visible)  //for thouse graphs who's  solid lines are visible,
                //                .Select(pair => pair.MarkerGraph.Visibility = Visibility.Visible);//show marker lines
            }
        }


        public void clearAll()
        {
            foreach (var pair in GraphsCollection)
            {
                _plotter.Children.Remove(pair.LineGraph);
                _plotter.Children.Remove(pair.MarkerGraph);
            }
            GraphsCollection.Clear();
            hideDefaultLegend();

        }

        public void removeGraph(object data)
        {
            if (data.GetType() != typeof(LineAndMarker<MarkerPointsGraph>))
                return;
            LineAndMarker<MarkerPointsGraph> pair = (LineAndMarker<MarkerPointsGraph>)data;
            if (!GraphsCollection.Contains(pair))
                return;
            //graph and markers are removed from collection and children
            _plotter.Children.Remove(pair.LineGraph);
            _plotter.Children.Remove(pair.MarkerGraph);
            GraphsCollection.Remove(pair);
            hideDefaultLegend();
        }


        //Colverts object data -> List<double>
        public void buildGraphFromYPoints(object data)
        {
            if (data.GetType() == typeof(List<ushort>))
            {
                List<double> yPoints = ((List<ushort>)data).Select(i => (double)i).ToList();
                buildGraphFromYPoints(yPoints);
            }
            else if (data.GetType() == typeof(List<int>))
            {
                List<double> yPoints = ((List<int>)data).Select(i => (double)i).ToList();
                buildGraphFromYPoints(yPoints);
            }
            else if (data.GetType() == typeof(List<double>))
            {
                buildGraphFromYPoints((List<double>)data);
            }

        }



        public void showLegend()
        {
            _logger.Log("showLegend: Command is not used any longer!", Category.Exception, Priority.Medium);
            //IRegion legendRegion = _regionManager.Regions[RegionNames.LegendRegion];
            //var view = legendRegion.GetView("LegendListKey");
            //if (view == null)
            //{
            //    var vm = _viewModelProvider.GetViewModel<LegendListViewModel>("LegendListKey");
            //    legendRegion.Add(new LegendListView() { DataContext = vm }, "LegendListKey");
            //}
            //view = legendRegion.GetView("LegendListKey");
            //legendRegion.Activate(view);
        }

        private void buildGraphFromYPoints(List<double> yPoints)
        {
            List<double> xPoints = generateXArray(yPoints);

            //Dymamic Data  Dispaly works with this source
            CompositeDataSource dataSource = xPoints.ToChartDataSource(yPoints);
            var graph = _plotter.AddLineGraph(dataSource, new Pen(randomBrush(), 2), new CirclePointMarker { Size = 5, Fill = Brushes.Red }, new PenDescription(getRandomName()));
            graph.MarkerGraph.Visibility = DEFAULT_MARKER_VILIBILITY;

            //var vm = _viewModelProvider.GetViewModel<LegendListViewModel>("LegendListKey");
            //vm.
            _legendListViewModel.addNewLegend(graph);
            GraphsCollection.Add(graph);
            //CustomCommands.AddNewLegend.Execute(graph, App.Current.MainWindow);
        }

        //private void buildGraphFromDataSource(CompositeDataSource dataSource)
        //{
        //    var graph = _plotter.AddLineGraph(dataSource, new Pen(randomBrush(), 2), new CirclePointMarker { Size = 5, Fill = Brushes.Red }, new PenDescription(getRandomName()));
        //    GraphsCollection.Add(graph);

        //}

        int graphNumber = 0;
        private string getRandomName()
        {
            return "Graph " + graphNumber++;
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
        public Color ColorFromHSV(double hue, double saturation, double value)
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
            double[] x = Enumerable.Range(0, yPoints.Count).Select(i => i / 1.0).ToArray();
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
        //Compleatly wrong! Need to redo
        public void show()
        {

            IRegion main = _regionManager.Regions[RegionNames.MainRegion];
            var view = main.GetView("PlotterKey");
            if (view == null)
            {
                main.Add(_chartView, "PlotterKey");
            }
            view = main.GetView("PlotterKey");
            main.Activate(view);
        }



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



        public void Initialize()
        {

        }


    }
}
