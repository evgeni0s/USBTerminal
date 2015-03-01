using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using USBTetminal2.Commands;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using USBTetminal2.Graphs;

namespace USBTetminal2.Controls.Legend
{
    /// <summary>
    /// This class is a bit complex. It provides data for list and items inside that list
    /// LegendListViewModel - contains list of items
    /// LegendListItem - items themselves
    /// </summary>
    public class LegendListViewModel : ViewModelBase, ILegendListViewModel
    {


        #region Class for ListItem
        /// <summary>
        /// Data for every particular item
        /// </summary>
        public class LegendListItem : ViewModelBase
        {
            private LineAndMarker<MarkerPointsGraph> _graph;
            private string description;
            private SolidColorBrush lineColor;
            public bool isChecked;
            private ILegendListViewModel _legendList;
            public LegendListItem(LineAndMarker<MarkerPointsGraph> graph, ILegendListViewModel legendList)
            {
                _graph = graph;
                Description = graph.LineGraph.Description.ToString();
                LineColor = graph.LineGraph.Stroke as SolidColorBrush; 
                IsChecked = true;
                _legendList = legendList;
            }

            //private LegendListItem _empty;
            private LegendListItem()
            {
                //_empty = new LegendListItem();
            }

            public static LegendListItem Empty
            {
                get { return new LegendListItem(); }
            }

            public string Description
            {
                get { return description; }
                set
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }

            public bool IsChecked
            {
                get { return isChecked; }
                set
                {
                    isChecked = value;
                    _graph.LineGraph.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    _graph.MarkerGraph.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    OnPropertyChanged("IsChecked");
                }
            }

            public SolidColorBrush LineColor
            {
                get { return lineColor; }
                set
                {
                    lineColor = value;
                    OnPropertyChanged("LineColor");
                }
            }

            public LineAndMarker<MarkerPointsGraph> Graph
            {
                get
                {
                    return _graph;
                }
            }

            public CompositeDataSource GraphPoints
            {
                get
                {
                   CompositeDataSource rawData = (CompositeDataSource)_graph.LineGraph.DataSource;
                   return rawData;
                }
            }

            public IEnumerable<Point> Points
            {
                get
                {
                    if (_graph == null || _graph.LineGraph == null || _graph.LineGraph.DataSource == null)
                        return new Point[0];
                    return _graph.LineGraph.DataSource.GetPoints();
                }
            }

            #region Commads
            
            private ICommand _removeLegendCommand;
            public ICommand RemoveLegendCommand
            {
                get { return _removeLegendCommand ?? (_removeLegendCommand = new RelayCommand(_legendList.deleteLegend)); }
            }

            //private ICommand _removeLegendCommand;
            //public ICommand RemoveLegendCommand
            //{
            //    get { return _removeLegendCommand ?? (_removeLegendCommand = new RelayCommand(_legendList.deleteLegend)); }
            //}
            #endregion
        }

        #endregion


        Visibility _LegendVisibility = Visibility.Visible;
        IGraphModule _graph;
        public void SetGraph(IGraphModule graph)
        {
            _graph = graph;
        }


        public Visibility LegendVisibility
        {
            get
            {
                if (LegendsList.Count <= 0)
                    return Visibility.Collapsed;
                return _LegendVisibility;
            }
            set 
            {
                _LegendVisibility = value;
                OnPropertyChanged("LegendVisibility");
            }
        }

        private ObservableCollection<LegendListItem> _legendsList = new ObservableCollection<LegendListItem>();
        public ObservableCollection<LegendListItem> LegendsList
        {
            get
            {
                return _legendsList;
            }
            set
            {
                OnPropertyChanged("LegendVisibility");
                _legendsList = value;
            }
        }

        private LegendListItem _selectedLegend;
        public LegendListItem SelectedLegend
        {
            get
            {
                return _selectedLegend ?? LegendListItem.Empty;
            }
            set
            {
                _selectedLegend = value;
                OnPropertyChanged("SelectedLegend");
            }
        }
         
        public void addNewLegend(object data)
        {
            if (data.GetType() != typeof(LineAndMarker<MarkerPointsGraph>))
                return;
            var graph = (LineAndMarker<MarkerPointsGraph>)data;
            LegendListItem legend = new LegendListItem(graph, this);
            LegendsList.Add(legend);
            OnPropertyChanged("LegendVisibility");
        }


        public void deleteLegend(object data)
        {
            if (data.GetType() != typeof(LegendListItem))
                return;
            LegendListItem item = (LegendListItem)data;
            LegendsList.Remove((LegendListItem)data);
            OnPropertyChanged("LegendVisibility");
            //CustomCommands.RemoveGraph.Execute(item.Graph, App.Current.MainWindow);
            _graph.removeGraph(item.Graph);
        }


        public void clearAll()
        {
            LegendsList.Clear();// = new ObservableCollection<LegendListItem>();
            OnPropertyChanged("LegendVisibility");
        }


        public void switchLegendVisibility(object cmdParam = null)
        {
            if (LegendVisibility == Visibility.Collapsed)
                LegendVisibility = Visibility.Visible;

            else if(LegendVisibility == Visibility.Visible)
                LegendVisibility = Visibility.Collapsed;
        }


        private ICommand _switchVisibilityCommand;
        public ICommand SwitchVisibilityCommand
        {
            get { return _switchVisibilityCommand ?? (_switchVisibilityCommand = new RelayCommand(switchLegendVisibility)); }
        }

      

        //public void ReciveMessage(CommonBroadcastType smgType, object data)
        //{
        //    switch (smgType)
        //    {
        //        case CommonBroadcastType.ADD_LEGEND_TO_GRAPH:
        //            addNewLegend(data);
        //            break;
        //        case CommonBroadcastType.DELETE_LEGEND:
        //            deleteLegend(data);
        //            break;
        //        case CommonBroadcastType.CLEAR_ALL:
        //            clearAll();
        //            break;
        //        case CommonBroadcastType.USER_CHANGED_LEGEND_CONTAINER_VISIBILITY:
        //            switchLegendVisibility();
        //            break;
        //        default:
        //            break;
        //    }
        //}




    }
}
